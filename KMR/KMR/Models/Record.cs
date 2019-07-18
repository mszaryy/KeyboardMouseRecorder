using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMR.Models
{
    class Record
    {
        public List<Step> StepsRecord { get; set; }


        public Record()
        {
            this.StepsRecord = new List<Step>();
        }


        public void AddRecord(Step step)
        {
            this.StepsRecord.Add(step);
        }
    }
}
