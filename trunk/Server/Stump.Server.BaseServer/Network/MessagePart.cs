using System;
using Stump.Core.IO;

namespace Stump.Server.BaseServer.Network
{
    class MessagePart
    {
        /// <summary>
        /// Set to true when the message is whole
        /// </summary>
        public bool IsValid
        {
            get
            {
                return Header.HasValue && Length.HasValue &&
                       Length == Data.Length;
            }
        }

        public int? Header
        {
            get;
            private set;
        }

        public int? MessageId
        {
            get
            {
                if (!Header.HasValue)
                    return null;

                return Header >> 2; // xxxx xx??
            }
        }

        public int? LengthBytesCount
        {
            get
            {
                if (!Header.HasValue)
                    return null;

                return Header & 0x3; // ???? ??xx
            }
        }

        public int? Length
        {
            get;
            private set;
        }

        private byte[] m_data;

        public byte[] Data
        {
            get { return m_data; }
            private set { m_data = value; }
        }

        /// <summary>
        /// Build or continue building the message. Returns true if the resulted message is valid and ready to be parsed
        /// </summary>
        public bool Build(BigEndianReader reader)
        {
            if (IsValid)
                return true;

            if (reader.BytesAvailable > 2 && !Header.HasValue)
            {
                Header = reader.ReadShort();
            }

            if (LengthBytesCount.HasValue &&
                reader.BytesAvailable > LengthBytesCount && !Length.HasValue)
            {
                if (LengthBytesCount < 0 || LengthBytesCount > 3)
                    throw new Exception("Malformated Message Header, invalid bytes number to read message length (inferior to 0 or superior to 3)");

                if (LengthBytesCount == 0)
                    Length = 0;
                
                // 3..0 or 2..0 or 1..0
                for (int i = LengthBytesCount.Value - 1; i <= 0; i--)
                {
                    Length |= reader.ReadByte() << (i * 8);
                }
            }

            // first case : no data read
            if (Data == null && Length.HasValue)
            {
                // enough bytes in the buffer to build a complete message
                if (reader.BytesAvailable >= Length)
                {
                    Data = reader.ReadBytes(Length.Value);
                }
                // not enough bytes, so we read what we can
                else if (Length > reader.BytesAvailable)
                {
                    Data = reader.ReadBytes((int) reader.BytesAvailable);
                }
            }
            //seconde case : the message was splitted and it felt some bytes
            if (Data != null && Length.HasValue && Data.Length < Length)
            {
                // still felt some bytes ...
                if (Data.Length + reader.BytesAvailable < Length)
                {
                    Array.Resize(ref m_data, (int)( Data.Length + reader.BytesAvailable ));
                    Array.Copy(reader.ReadBytes((int)reader.BytesAvailable), 0, Data, Data.Length, reader.BytesAvailable);
                }
                // there is enough bytes in the buffer to complete the message :)
                if (Data.Length + reader.BytesAvailable >= Length)
                {
                    int bytesToRead = Length.Value - Data.Length;
                    Array.Resize(ref m_data, Data.Length + bytesToRead);
                    Array.Copy(reader.ReadBytes(bytesToRead), 0, Data, Data.Length, bytesToRead);
                }
            }

            return IsValid;
        }
    }
}