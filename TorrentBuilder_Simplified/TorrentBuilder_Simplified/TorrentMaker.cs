using EAD.Torrent;
using System;
using System.Collections;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace TorrentBuilder_Simplified
{
    class TorrentMaker
    {
        int bufferPosition = 0;
        byte[] buffer;
        int currentPieceSize;
        readonly SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();
        TorrentBuild parent;

        byte[] sha1_temp = new byte[0x15];
        StringBuilder sha1_full = new StringBuilder(80);


        public TorrentMaker(TorrentBuild tb, int pieceSize)
        {
            this.parent = tb;
            this.currentPieceSize = pieceSize;
            this.buffer = new byte[this.currentPieceSize];
        }
        private void MakeHash(string fullPath)
        {
            //Makes a hash from a file individually.
            FileStream stream;
            long toRead = (long)this.currentPieceSize;
            long fileStreamPosition = 0;

            FileInfo myFile = new FileInfo(fullPath);
            long fileSize = myFile.Length;

            long bytesRead = 0;

            using (stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                stream.Position = fileStreamPosition;

                while (stream.Position < fileSize)
                {
                    if (fileSize - fileStreamPosition < toRead)
                    {
                        toRead = fileSize - (long)fileStreamPosition;
                        Array.Resize(ref buffer, (int)toRead);
                    }
                    bytesRead = stream.Read(buffer, 0, (int)toRead);
                    fileStreamPosition += toRead;
                    stream.Position = fileStreamPosition;
                    sha1_temp = sha1.ComputeHash(buffer);
                    sha1_full.Append(Encoding.Default.GetString(sha1_temp));

                    parent.methodUpdateBar(bytesRead);

                }
            }
        }

        private void MakeContinuousHash(string fullPath, bool isLastFile)
        {
            //Makes a hash from every file in the folder as if the bytes from all the files were concatenated.
            FileStream stream;

            long toRead;
            long fileStreamPosition = 0;

            FileInfo myFile = new FileInfo(fullPath);
            long filesize = myFile.Length;

            int bytesRead;

            using (stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                stream.Position = fileStreamPosition;

                while (stream.Position < filesize)
                {
                    //The number of bytes to be read, depends on the size available in the buffer and on the size remaining to be read in the file.
                    //Chooses the smaller one.
                    toRead = Math.Min(filesize - fileStreamPosition, this.currentPieceSize - bufferPosition);
                    bytesRead = stream.Read(buffer, bufferPosition, (int)toRead);

                    bufferPosition += bytesRead;
                    fileStreamPosition += bytesRead;
                    stream.Position = fileStreamPosition;

                    //If the buffer has been completely filled, starts to hash and append.
                    if (bufferPosition == currentPieceSize)
                    {
                        sha1_temp = sha1.ComputeHash(buffer);
                        sha1_full.Append(Encoding.Default.GetString(sha1_temp));

                        parent.methodUpdateBar(bytesRead);

                        bufferPosition = 0;
                    }
                }

                //If the current file is the last one and after it has been completely buffered, 
                //hashes the content in buffer even if it's smaller than piece size.
                if (isLastFile)
                {
                    //Removes extra bytes allocated if the buffer size is smaller than piece size
                    //to avoid hashing wrong bytes.
                    Array.Resize(ref buffer, bufferPosition);

                    sha1_temp = sha1.ComputeHash(buffer);
                    sha1_full.Append(Encoding.Default.GetString(sha1_temp));

                    parent.methodUpdateBar(bufferPosition);

                }
            }
        }

        public void MakeTorrentFromFile(string fullPath)
        {
            FileInfo myFile = new FileInfo(fullPath);
            long filesize = myFile.Length;

            this.MakeHash(fullPath);

            int pathNameSize = fullPath.LastIndexOf(@"\");
            int fileNameSize = fullPath.Length - pathNameSize;

            string fileName = fullPath.Substring(pathNameSize + 1, fileNameSize - 1);
            string fullPathToSave = parent.folderToSave + "/" + fileName;

            TorrentDictionary mainDict = new TorrentDictionary();
            TorrentDictionary infoDict = new TorrentDictionary();
            TorrentString torr_name = new TorrentString
            {
                Value = fileName
            };
            TorrentNumber torr_length = new TorrentNumber
            {
                Value = filesize
            };
            TorrentNumber torr_piece_size = new TorrentNumber
            {
                Value = this.currentPieceSize
            };
            TorrentString torr_encode = new TorrentString
            {
                Value = "UTF-8"
            };
            TorrentString torr_pieces = new TorrentString
            {
                Value = sha1_full.ToString()
            };
            TorrentString torr_createdBy = new TorrentString
            {
                Value = "Criado por TorrentBuild Simplificado."
            };

            infoDict.Add("length", torr_length);
            infoDict.Add("name", torr_name);
            infoDict.Add("pieces", torr_pieces);
            infoDict.Add("piece length", torr_piece_size);
            mainDict.Add("created by", torr_createdBy);
            mainDict.Add("encoding", torr_encode);
            mainDict.Add("info", infoDict);

            using (FileStream fs = File.Open(fullPathToSave + ".torrent", FileMode.OpenOrCreate))
            {
                byte[] bencodedDict = Encoding.ASCII.GetBytes(mainDict.BEncoded);
                fs.Write(bencodedDict, 0, bencodedDict.Length);
            }
        }

        public void MakeTorrentFromFolder(string fullPath)
        {
            string[] pathArray = Directory.GetFiles(fullPath, "*", SearchOption.AllDirectories);
            string[] relative_pathArray = new string[pathArray.Length];

            int pathNameSize = fullPath.LastIndexOf(@"\");
            int fileNameSize = fullPath.Length - pathNameSize;

            string fileName = fullPath.Substring(pathNameSize + 1, fileNameSize - 1);
            string fullPathToSave = parent.folderToSave + @"\" + fileName;

            TorrentDictionary mainDict = new TorrentDictionary();
            TorrentDictionary infoDict = new TorrentDictionary();

            ArrayList fileArrayList = new ArrayList();
            TorrentList torr_fileList = new TorrentList();

            for (int i = 0; i < pathArray.Length; i++)
            {
                FileInfo myFile = new FileInfo(pathArray[i]);
                long filesize = myFile.Length;

                relative_pathArray[i] = pathArray[i].Substring(fullPath.Length + 1);

                string[] path_splitted = relative_pathArray[i].Split('\\');
                TorrentNumber torr_filesize = new TorrentNumber { Value = filesize };

                ArrayList pathArrayList = new ArrayList();

                TorrentDictionary itemDict = new TorrentDictionary();

                foreach (string path_chunk in path_splitted)
                {
                    TorrentString path_chunkString = new TorrentString { Value = path_chunk };
                    pathArrayList.Add(path_chunkString);
                }

                TorrentList torr_pathList = new TorrentList { Value = pathArrayList };

                itemDict.Add("length", torr_filesize);
                itemDict.Add("path", torr_pathList);

                fileArrayList.Add(itemDict);
                if (i == pathArray.Length - 1)
                {
                    MakeContinuousHash(pathArray[i], true);
                }
                else
                {
                    MakeContinuousHash(pathArray[i], false);
                }
            }

            torr_fileList.Value = fileArrayList;
            infoDict.Add("files", torr_fileList);

            TorrentString torr_pieces = new TorrentString { Value = sha1_full.ToString() };
            infoDict.Add("pieces", torr_pieces);

            TorrentNumber torr_pieceLength = new TorrentNumber { Value = this.currentPieceSize };
            infoDict.Add("piece length", torr_pieceLength);

            TorrentString torr_name = new TorrentString { Value = fileName };
            infoDict.Add("name", torr_name);

            TorrentString torr_createdBy = new TorrentString { Value = "Criado por TorrentBuild Simplificado." };
            mainDict.Add("created by", torr_createdBy);

            TorrentString torr_encode = new TorrentString { Value = "UTF-8" };
            mainDict.Add("encoding", torr_encode);

            mainDict.Add("info", infoDict);

            using (FileStream fs = File.Open(fullPathToSave + ".torrent", FileMode.OpenOrCreate))
            {
                byte[] bencodedDict = Encoding.ASCII.GetBytes(mainDict.BEncoded);
                fs.Write(bencodedDict, 0, bencodedDict.Length);
            }
        }
    }
}
