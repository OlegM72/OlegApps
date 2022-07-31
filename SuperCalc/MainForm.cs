using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperCalc
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            // Create two buttons to use as the accept and cancel buttons.
            Button button1 = new Button();
            Button button2 = new Button();

            // Set the text of button1 to "OK".
            button1.Text = "Calculate";
            // Set the position of the button on the form.
            button1.Location = new Point(10, 10);

            button1.Click += OnButton1Click;
            button2.Click += OnButton2Click;

            // Set the text of button2 to "Cancel".
            button2.Text = "Exit";
            // Set the position of the button based on the location of button1.
            button2.Location
               = new Point(button1.Left, button1.Height + button1.Top + 10);
            // Set the caption bar text of the form.   
            Text = "My Dialog Box";
            // Display a help button on the form.
            HelpButton = true;

            // Define the border style of the form to a dialog box.
            FormBorderStyle = FormBorderStyle.FixedDialog;
            // Set the MaximizeBox to false to remove the maximize box.
            MaximizeBox = false;
            // Set the MinimizeBox to false to remove the minimize box.
            MinimizeBox = false;
            // Set the accept button of the form to button1.
            AcceptButton = button1;
            // Set the cancel button of the form to button2.
            CancelButton = button2;
            // Set the start position of the form to the center of the screen.
            StartPosition = FormStartPosition.CenterScreen;

            // Add button1 to the form.
            Controls.Add(button1);
            // Add button2 to the form.
            Controls.Add(button2);

            // Display the form as a modal dialog box.
            ShowDialog();
        }

        protected virtual void OnButton1Click(object sender, EventArgs e)
        {
            Program.Calculate("../../../Expressions.txt", "../../../CorrectResultsForCheck.txt");
        }

        protected virtual void OnButton2Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
