using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLove.Interfaces
{
    public interface ISpeechToText
    {
        void RecordMic();
        void stopListening();
    }

}
