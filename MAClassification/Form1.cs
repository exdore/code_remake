using System.Windows.Forms;


namespace MAClassification   
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var data = Table.ReadData();
            var attributes = data.GetAttributesInfo();
        }
    }
}
