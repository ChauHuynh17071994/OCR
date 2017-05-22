using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.CV.OCR;
//using tessnet2;
namespace DemoOCR
{
    public partial class Form1 : Form
    {
        // variable
        Capture capwebcam = null;
        bool blnCapturingInprocess = false;
        Image<Bgr, byte> imgOriginal;
        private string m_path = Application.StartupPath + @"\data\";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClose(object sender, FormClosedEventArgs e)
        {
            if (capwebcam != null)
            {
                capwebcam.Dispose();
            }
        }
        void processFrameAndUpdateGUI(object sender, EventArgs arg)
        {
            imgOriginal = capwebcam.QueryFrame(); // get the next frame from the webcame
            if (imgOriginal == null) return;     // if we did not get a frame.
            Tesseract ocr = new Tesseract(@"C:\Emgu\emgucv-windows-x86 2.4.0.1717\bin\tessdata", "eng", Tesseract.OcrEngineMode.OEM_TESSERACT_ONLY);
            ocr.Recognize(imgOriginal);
            if (rtb1.Text != "") rtb1.AppendText(Environment.NewLine);
            rtb1.AppendText(ocr.GetText());
           

            imageBox1.Image = imgOriginal;
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            try
            {
                capwebcam = new Capture();  // associate capture object to the default webcam
            }
            catch (NullReferenceException except)//catch error if unsucessful
            {
                rtb1.Text = except.Message;  // show error ohject message in text box
                return;
            }                               //Once we have a good capture object...

            Application.Idle += processFrameAndUpdateGUI; // add process image function to the application's list of task
            blnCapturingInprocess = true; //update member flag variable
        }

        private void btnPause_Click_1(object sender, EventArgs e)
        {
            if (blnCapturingInprocess == true)       // if we are currently, processing an image, user just chose pause to....
            {
                Application.Idle -= processFrameAndUpdateGUI;//remove the process image funtion from the application's list of tasks 
                blnCapturingInprocess = false; // update flage variable
                btnPause.Text = "Resume";      //update button text
            }
            else                                 // else if we are not currently processing an imge, user just chose resume to....
            {
                Application.Idle += processFrameAndUpdateGUI; //add the process image function to the application's list of tasks
                blnCapturingInprocess = true;                   // update flag variable
                btnPause.Text = "Pause";                        // now button will offer pause option
            }
        }
    }
}
