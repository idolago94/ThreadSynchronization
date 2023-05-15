using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadSynchronization
{
    //This class extends the MailBox, by overriding the read and write methods.
    //The override must call the original implementation, which contain races, protecting the critical sections from races.
    //You cannot define a new message array or any other data structue for messages here.
    //You can, however, add any other fields as you see fit.
    class SynchronizedMailBox : MailBox
    {
        private Mutex mu = new Mutex();
        private Semaphore se = new Semaphore(0);

        public override void Write(Message msg)
        {
            mu.Lock();
            base.Write(msg);
            mu.Unlock();
            se.Up();
        }
        public override Message Read()
        {
            if (base.IsEmpty()) se.Down();
            mu.Lock();
            Message msg = base.Read();
            mu.Unlock();
            return msg;
        }
    }
}
