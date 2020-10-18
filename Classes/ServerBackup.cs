using System;
using System.Collections.Generic;
using System.Text;

namespace PokeParadise.Classes
{
    [Serializable]
    public class ServerBackup
    {
        public DateTimeOffset lastBackup;
        public int amountOfBackups;

        public ServerBackup()
        {

        }

        public ServerBackup(DateTimeOffset lastBackup, int amountOfBackups)
        {
            this.lastBackup = lastBackup;
            this.amountOfBackups = amountOfBackups;
        }
    }
}
