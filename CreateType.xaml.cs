using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace budget
{
    public partial class CreateType : Window
    {
        public string TypeName { get; set; }

        public CreateType()
        {
            InitializeComponent();
        }

        private void addType_Click(object sender, RoutedEventArgs e)
        {
            TypeName = type_text.Text;
            this.Close();
        }
    }
}
