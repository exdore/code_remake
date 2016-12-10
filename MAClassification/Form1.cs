using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;


namespace MAClassification   
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            const int maxAntsNumber = 10;
            const int maxNumberForConvergence = 3;
            const int maxUncoveredCases = 2;
            const int minCasesPerRule = 2;
            InitializeComponent();
            var data = Table.ReadData();
        }
    }
}
