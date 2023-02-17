using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSVTokenOperations
{
    public class FileHelper
    {


        [Serializable, StructLayout(LayoutKind.Sequential)]
        private struct WIN32_FIND_DATA
        {
            public int dwFileAttributes;
            public int ftCreationTime_dwLowDateTime;
            public int ftCreationTime_dwHighDateTime;
            public int ftLastAccessTime_dwLowDateTime;
            public int ftLastAccessTime_dwHighDateTime;
            public int ftLastWriteTime_dwLowDateTime;
            public int ftLastWriteTime_dwHighDateTime;
            public int nFileSizeHigh;
            public int nFileSizeLow;
            public int dwReserved0;
            public int dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
            public string cAlternateFileName;
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr FindFirstFile(string pFileName, ref WIN32_FIND_DATA pFindFileData);
        [DllImport("kernel32.dll")]
        private static extern bool FindNextFile(IntPtr hFindFile, ref WIN32_FIND_DATA lpFindFileData);
        [DllImport("kernel32.dll")]
        private static extern bool FindClose(IntPtr hFindFile);

        private static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
        private const int FILE_ATTRIBUTE_DIRECTORY = 16;

        public long GetFileCount(string dir, bool includeSubdirectories = false)
        {
            string searchPattern = Path.Combine(dir, "*.csv");

            var findFileData = new WIN32_FIND_DATA();
            IntPtr hFindFile = FindFirstFile(searchPattern, ref findFileData);
            if (hFindFile == INVALID_HANDLE_VALUE)
                throw new Exception("Directory not found: " + dir);

            long fileCount = 0;
            do
            {
                //if (findFileData.dwFileAttributes != FILE_ATTRIBUTE_DIRECTORY)
                if ((findFileData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) == 0)
                {
                    fileCount++;
                    continue;
                }

                if (includeSubdirectories && findFileData.cFileName != "." && findFileData.cFileName != "..")
                {
                    string subDir = Path.Combine(dir, findFileData.cFileName);
                    fileCount += GetFileCount(subDir, true);
                }
            }
            while (FindNextFile(hFindFile, ref findFileData));

            FindClose(hFindFile);

            return fileCount;
        }

        public List<string> GetFileNames(string dir, bool includeSubdirectories = false)
        {
            string searchPattern = Path.Combine(dir, "*.csv");

            var findFileData = new WIN32_FIND_DATA();
            IntPtr hFindFile = FindFirstFile(searchPattern, ref findFileData);
            if (hFindFile == INVALID_HANDLE_VALUE)
                throw new Exception("Directory not found: " + dir);

            List<string> fileList = new ();
            do
            {
                //if (findFileData.dwFileAttributes != FILE_ATTRIBUTE_DIRECTORY)
                if ((findFileData.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY) == 0)
                {
                    fileList.Add(findFileData.cFileName);
                    continue;
                }

                if (includeSubdirectories && findFileData.cFileName != "." && findFileData.cFileName != "..")
                {
                    string subDir = Path.Combine(dir, findFileData.cFileName);
                    fileList.AddRange(GetFileNames(subDir, true));
                }
            }
            while (FindNextFile(hFindFile, ref findFileData));

            FindClose(hFindFile);

            return fileList;
        }

    }
}