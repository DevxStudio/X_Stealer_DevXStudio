// By https://t.me/devxstudio
// https://t.me/devx_channel
// SHOP MALWARE: https://t.me/devxstudiobot
namespace XStealer.Helpers
{
    using System;

    public static class Enums
    {
        [Flags]
        public enum Type : int
        {
            Sequence = 0x30,
            Integer = 0x02,
            BitString = 0x03,
            OctetString = 0x04,
            Null = 0x05,
            ObjectIdentifier = 0x06
        }

        [Flags]
        public enum JsonType : int
        {
            String = 0,
            Number = 1,
            Object = 2,
            Array = 3,
            Boolean = 4
        }

        [Flags]
        public enum AdminPromptType : uint
        {
            AllowAll,
            DimmedPromptWithPasswordConfirmation,
            DimmedPrompt,
            PromptWithPasswordConfirmation,
            Prompt,
            DimmedPromptForNonWindowsBinaries
        }
    }
}