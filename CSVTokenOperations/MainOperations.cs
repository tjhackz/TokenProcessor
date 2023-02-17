using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static CSVTokenOperations.CSVModels;

namespace CSVTokenOperations
{
    public class MainOperations
    {
        public const string DELETED_DIR_NAME = "deleted";

        public static IEnumerable<MasterCSV_Model>? ReadMasterCSV(string MasterDirPath, string MasterFileName)
        {
            string fullPath = System.IO.Path.Combine(MasterDirPath, MasterFileName); // MasterDirPath + "/" + MasterFileName;
            try
            {
                using (var reader = new StreamReader(fullPath))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<MasterCSV_Model>().ToList();
                    return records;
                }
            }
            catch (Exception ex)
            {
                //something wrong happend, log it
                return null;
            }
        }

        public static DeleteFlagEnum GetDeleteFlagValueFromMasterModel(IEnumerable<MasterCSV_Model>? masterCSVModels)
        {
            if (masterCSVModels?.Count() > 0 && masterCSVModels?.FirstOrDefault() != null)
            {
                if (!string.IsNullOrEmpty(masterCSVModels?.FirstOrDefault()?.DeleteFlag)
                    && !string.IsNullOrWhiteSpace(masterCSVModels?.FirstOrDefault()?.DeleteFlag)
                    && !string.IsNullOrEmpty(masterCSVModels?.FirstOrDefault()?.DeleteFlag?.Trim()))
                {
                    return (masterCSVModels?.FirstOrDefault()?.DeleteFlag) switch
                    {
                        "Y" => DeleteFlagEnum.Y,
                        "N" => DeleteFlagEnum.N,
                        " " => DeleteFlagEnum.BLANK,
                        _ => DeleteFlagEnum.OTHER,
                    };
                }
                else
                {
                    return DeleteFlagEnum.BLANK;
                }
            }
            else
            {
                //NO RECORD FOUND
                // Log this may be
                return DeleteFlagEnum.OTHER;
            }

        }

        public static bool CheckIfDeleteFlagValid(DeleteFlagEnum flag)
        {
            if(flag == DeleteFlagEnum.Y || flag == DeleteFlagEnum.BLANK)
            {
                return true;
            }
            return false;
        }

        private static void CreateDirectory(string path)
        {
            try
            {
                System.IO.Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                //log error may be?
            }
        }

        public static void MoveCSVFile(string SourcePath, string DestinationPath, string FileName)
        {            
            string sourceFile = System.IO.Path.Combine(SourcePath, FileName);
            string destFile = System.IO.Path.Combine(DestinationPath, FileName);
            CreateDirectory(DestinationPath);
            System.IO.File.Move(sourceFile, destFile, true);
        }

        public static bool CheckIfFileExistInDirectory(string DirPath, string FileName)
        {
            return File.Exists(System.IO.Path.Combine(DirPath, FileName));
        }

        public static bool CheckIfFileExistInDirectory(string FileName, List<string> FileList)
        {
            if (FileList?.Count > 0)
            {
                if (FileList.Any(x => x.Trim().ToLower().Equals(FileName.Trim().ToLower())))
                {
                    return true;
                }
            }
            return false;
        }


    }
}
