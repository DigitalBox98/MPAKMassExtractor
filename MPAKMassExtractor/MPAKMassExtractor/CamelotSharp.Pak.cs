using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Xml;

using ICSharpCode.SharpZipLib.Zip.Compression;

namespace CamelotSharp.Pak
{
    /// <summary>
    /// PAKFile is a class used to access files in MPAK archives (files with the mpk and npk extensions).
    /// </summary>
    public class PAKFile
    {
        private Stream archiveFile;
        private string archivePath;

        private BinaryReader reader;

        private string internalArchiveName = "";
        private Hashtable entriesTable = new Hashtable();
        /// <summary>
        /// Get the internal archive name.
        /// </summary>
        public string ArchiveName
        {
            get
            {
                return internalArchiveName;
            }
        }
        /// <summary>
        /// Initializes the PAKFile class with the specified archive.
        /// </summary>
        /// <param name="filePath">The archive file path.</param>
        public PAKFile(string filePath)
        {
            archiveFile = File.OpenRead(filePath);
            archivePath = filePath;
            processPAKFile();
        }
        /// <summary>
        /// Initializes the PAKFile class with the specified archive
        /// </summary>
        /// <param name="fileStream">The archive stream.</param>
        public PAKFile(Stream fileStream)
        {
            archiveFile = fileStream;

            processPAKFile();
        }
        private void processPAKFile()
        {
            reader = new BinaryReader(archiveFile);
            string head = ASCIIEncoding.ASCII.GetString(reader.ReadBytes(4));
            // Throw a FileLoadException
            if (head != "MPAK")
                throw (new FileLoadException("This file is not an MPAK file.", archivePath));
            else
            {
                // What is in the first 17 bytes?
                reader.BaseStream.Seek(21, SeekOrigin.Begin);
                // Expand the first stream: archive name
                internalArchiveName = System.Text.Encoding.ASCII.GetString(ReadStream());
                // Expand the second stream: directory
                byte[] directory = ReadStream();

                // Retrieve filenames and stream offsets from the directory
                // 0x11B = 283
                // 0x11C = 284
                // 0x110 = 272
                // 0x111 = 273
                // 0x112 = 274
                // 0x113 = 275

                DateTime t = new DateTime(18208454827L);
                long dirOffset = reader.BaseStream.Position;
                // each entry is 284bytes long
                for (int offset = 0; offset < directory.Length - 0x11b; offset += 0x11c)
                {
                    Entry entry = new Entry();
                    // The null terminated file name
                    StringBuilder name = new StringBuilder();
                    for (int i = 0; directory[offset + i] != 0; i++)
                        name.Append((char)directory[offset + i]);
                    entry.FileName = name.ToString();
                    //At the end, we have:
                    entry.unknown3 =
                        ((ulong)directory[offset + 0x107] << 56) +
                        ((ulong)directory[offset + 0x106] << 48) +
                        ((ulong)directory[offset + 0x105] << 40) +
                        ((ulong)directory[offset + 0x104] << 32) +
                        ((ulong)directory[offset + 0x103] << 24) +
                        ((ulong)directory[offset + 0x102] << 16) +
                        ((ulong)directory[offset + 0x101] << 8) +
                        ((ulong)directory[offset + 0x100]);

                    entry.unknown1 =
                        ((long)(directory[offset + 0x10B] << 24) +
                        (long)(directory[offset + 0x10A] << 16) +
                        (long)(directory[offset + 0x109] << 8) +
                        (long)(directory[offset + 0x108]));
                    entry.UncompressedSize =
                        ((long)(directory[offset + 0x10F] << 24) +
                        (long)(directory[offset + 0x10E] << 16) +
                        (long)(directory[offset + 0x10D] << 8) +
                        (long)(directory[offset + 0x10C]));
                    // followed by the offset of the file as a uint on the last 4 bytes of the field
                    entry.FileOffset =
                        dirOffset +
                        ((directory[offset + 0x113] << 24) +
                        (directory[offset + 0x112] << 16) +
                        (directory[offset + 0x111] << 8) +
                        (directory[offset + 0x110]));
                    entry.CompressedSize =
                        ((long)(directory[offset + 0x117] << 24) +
                        (long)(directory[offset + 0x116] << 16) +
                        (long)(directory[offset + 0x115] << 8) +
                        (long)(directory[offset + 0x114]));
                    entry.unknown2 =
                        ((long)directory[offset + 0x11B] << 24) +
                        ((long)directory[offset + 0x11A] << 16) +
                        ((long)directory[offset + 0x119] << 8) +
                        ((long)directory[offset + 0x118]);
                    entry.dateCreation = new DateTime((long)(entry.unknown3 * 1000000));
                    // hum, what is the last 8 bytes here?
                    entriesTable.Add(name.ToString().ToLower(), entry);

                }
            }
        }
        /// <summary>
        /// Extracts the specified file name from the archive into a destination stream.
        /// </summary>
        /// <param name="fileToExtract">The file name to extract.</param>
        /// <param name="destinationStream">The Stream where to extract the file.</param>
        /// <returns>true if the operation succeeds, else false.</returns>
        public bool ExtractFile(String fileToExtract, Stream destinationStream)
        {
            byte[] data = new byte[1];
            if (ExtractFile(fileToExtract, ref data))
            {
                destinationStream.Write(data, 0, data.Length);
                return true;
            }
            else
                return false;
        }

        private bool ExtractFile(string FileName, ref byte[] DestinationArray)
        {
            try
            {
                DestinationArray = getEntryBytes(FileName);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Get the Files in the archive as a string array.
        //		/// </summary>
        //		public String[] FileNames
        //		{
        //			get
        //			{
        //				string[] s = new string[entriesTable.Keys.Count];
        //				entriesTable.Keys.CopyTo(s,0);
        //				return s;
        //			}
        //		}
        public Entry[] Files
        {
            get
            {
                Entry[] t = new Entry[entriesTable.Values.Count];
                entriesTable.Values.CopyTo(t, 0);
                return t;
            }
        }
        /// <summary>
        /// Close the PAKFile class and any associated Streams.
        /// </summary>
        public void Close()
        {
            if (reader != null)
            {
                reader.Close();
                reader = null;
            }
        }

        private byte[] getEntryBytes(String name)
        {
            if (reader == null)
                throw (new IOException("archive is closed"));

            string lowerName = name.ToLower();
            Entry entry = (Entry)entriesTable[lowerName];
            if (entry.FileOffset == 0)
                throw (new FileNotFoundException("no such entry"));

            reader.BaseStream.Seek(entry.FileOffset, SeekOrigin.Begin);
            return ReadStream();
        }
        private byte[] ReadStream()
        {
            Inflater inflater = new Inflater();
            byte[] input = new byte[1024];
            byte[] output = new byte[1024];
            int outputUsed = 0;

            while (!inflater.IsFinished)
            {
                while (inflater.IsNeedingInput)
                {
                    int count = reader.Read(input, 0, 1024);
                    if (count <= 0)
                        throw (new EndOfStreamException("Unexpected End Of File"));
                    inflater.SetInput(input, 0, count);
                }
                if (outputUsed == output.Length)
                {
                    byte[] newOutput = new byte[output.Length * 2];
                    Array.Copy(output, newOutput, output.Length);
                    output = newOutput;
                }
                try
                {
                    outputUsed += inflater.Inflate(output, outputUsed, output.Length - outputUsed);
                }
                catch (FormatException e)
                {
                    throw (new IOException(e.ToString()));
                }
            }

            // Adjust reader to point to the next stream correctly.
            reader.BaseStream.Seek(reader.BaseStream.Position - inflater.RemainingInput, SeekOrigin.Begin);
            inflater.Reset();

            byte[] realOutput = new byte[outputUsed];
            Array.Copy(output, realOutput, outputUsed);
            return realOutput;
        }
    }
    public struct Entry
    {
        public long unknown1;
        public long unknown2;
        public long CompressedSize;
        public ulong unknown3;
        public long UncompressedSize;
        internal long FileOffset;
        public string FileName;
        public DateTime dateCreation;
    }
}