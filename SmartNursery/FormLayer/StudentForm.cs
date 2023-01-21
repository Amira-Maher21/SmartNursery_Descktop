using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using SmartNursery.DatabaseLayer;

namespace SmartNursery.FormLayer
{
    public partial class StudentForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        DataClassesDataContext db = new DataClassesDataContext();
        public bool IsEdit;
        public int Code;
        public StudentForm()
        {
            InitializeComponent();
        }
        
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            Close();
        }


        private void Fill()
        {
            ClassId.Properties.DataSource = db.ClassTablees.ToList();
            PaymentTypeId.Properties.DataSource = db.PaymentTypes.ToList();
            IsActive.Checked = true;
            RegisterFees.Text = "0";
            StudentDateOfBirth.EditValue = DateTime.Now;
            StudentJoiningDate.EditValue = DateTime.Now;
            StudentBarCode.Text = (barCod() + 1).ToString();
        }

        public int barCod()
        {
            int code = 0;
            var data = db.BarCode();
            foreach (var item in data)
            {
                code = Convert.ToInt32(item.Column1);
            }
            return code;
        }
        
        private void Clear()
        {
            StudentName.Text = "";
            StudentDateOfBirth.EditValue = DateTime.Now;
            StudentAddress.Text = "";
            StudentPhone.Text = "";
            StudentJoiningDate.EditValue = DateTime.Now;
            StudentHealthStatus.Text = "";
            ClassId.EditValue = null;
            StudentPicture.EditValue = null;
            FatherName.Text = "";
            FatherAddress.Text = "";
            FatherPhone.Text = "";
            FatherJob.Text = "";
	        FatherEmail.Text="";
            MotherName.Text = "";
            MotherAddress.Text = "";
            MotherJob.Text = "";
            MotherPhone.Text = "";
            MotherEmail.Text = "";
            EmergencyPersonName.Text = "";
            EmergencyPersonPhone.Text = "";
            RegisterFees.Text = "0";
            PaymentTypeId.EditValue = null;
            Notes.Text = "";
            StudentBarCode.Text = "";
            DeductValue.Text = "";
            Age.Text= "";
            Fill();
        }

        
        private void InsertOrUpdate()
        {

            if (!dxValidationProvider1.Validate())
            {
                return;
            }
            if (!IsEdit)
            {
                Student st = new Student();
                {
                    st.StudentName = StudentName.Text;
                    st.StudentDateOfBirth = Convert.ToDateTime(StudentDateOfBirth.EditValue);
                    st.StudentAddress = StudentAddress.Text;
                    st.StudentPhone = StudentPhone.Text;
                    st.StudentJoiningDate = Convert.ToDateTime(StudentJoiningDate.EditValue);
                    st.StudentHealthStatus = StudentHealthStatus.Text;
                    st.ClassId = Convert.ToInt32(ClassId.EditValue);
                    Image img = StudentPicture.Image;
                    ImageConverter imgCo = new ImageConverter();
                    byte[] img2Arry = (byte[])imgCo.ConvertTo(img, typeof(byte[]));
                    st.StudentPicture = img2Arry;
                    st.FatherName = FatherName.Text;
                    st.FatherAddress = FatherAddress.Text;
                    st.FatherPhone = FatherPhone.Text;
                    st.FatherJob = FatherJob.Text;
                    st.FatherEmail = FatherEmail.Text;
                    st.MotherName = MotherName.Text;
                    st.MotherAddress = MotherAddress.Text;
                    st.MotherJob = MotherJob.Text;
                    st.MotherPhone = MotherPhone.Text;
                    st.MotherEmail = MotherEmail.Text;
                    st.EmergencyPersonName = EmergencyPersonName.Text;
                    st.EmergencyPersonPhone = EmergencyPersonPhone.Text;
                    st.RegisterFees = RegisterFees.Text;
                    st.PaymentTypeId = Convert.ToInt32(PaymentTypeId.EditValue);
                    st.Notes = Notes.Text;
                    st.StudentBarCode = StudentBarCode.Text;
                    st.DeductValue = DeductValue.Text;
                    st.Age = Age.Text;
                }
                db.Students.InsertOnSubmit(st);
                db.SubmitChanges();


            }
            else
            {
                var Update = db.Students.Where(p => p.StudentId == Code).FirstOrDefault();
                { 
                      Update.StudentName = StudentName.Text;
                      Update.StudentDateOfBirth = Convert.ToDateTime(StudentDateOfBirth.EditValue);
                      Update.StudentAddress = StudentAddress.Text;
                      Update.StudentPhone = StudentPhone.Text;
                      Update.StudentJoiningDate = Convert.ToDateTime(StudentJoiningDate.EditValue);
                      Update.StudentHealthStatus = StudentHealthStatus.Text;
                      Update.ClassId = Convert.ToInt32(ClassId.EditValue);
                      Image img = StudentPicture.Image;
                      ImageConverter imgCo = new ImageConverter();
                      byte[] img2Arry = (byte[])imgCo.ConvertTo(img, typeof(byte[]));
                      Update.StudentPicture = img2Arry;
                      Update.FatherName = FatherName.Text;
                      Update.FatherAddress = FatherAddress.Text;
                      Update.FatherPhone = FatherPhone.Text;
                      Update.FatherJob = FatherJob.Text;
                      Update.FatherEmail = FatherEmail.Text;
                      Update.MotherName = MotherName.Text;
                      Update.MotherAddress = MotherAddress.Text;
                      Update.MotherJob = MotherJob.Text;
                      Update.MotherPhone = MotherPhone.Text;
                      Update.MotherEmail = MotherEmail.Text;
                      Update.EmergencyPersonName = EmergencyPersonName.Text;
                      Update.EmergencyPersonPhone = EmergencyPersonPhone.Text;
                      Update.RegisterFees = RegisterFees.Text;

                    if (Update.PaymentTypeId == null )
                    {
                        
                    }
                    else
                    {
                        Update.PaymentTypeId = Convert.ToInt32(PaymentTypeId.EditValue);
                    }
                      
                      Update.Notes = Notes.Text;
                      Update.StudentBarCode = StudentBarCode.Text;
                      Update.DeductValue = DeductValue.Text;
                      Update.Age = Age.Text;
                }
                    db.SubmitChanges();
            }
            MessageBox.Show("تم الحفظ بنجاح");
            Clear();
        }
        private void StudentAddress_EditValueChanged(object sender, EventArgs e)
        {
                string Adress = StudentAddress.Text;
                MotherAddress.Text = Adress;
                FatherAddress.Text = Adress;
        }

        private void StudentForm_Load(object sender, EventArgs e)
        {
            Fill();

            if (IsEdit)
            {
                var Edit = db.Students.Where(s => s.StudentId == Code).FirstOrDefault();

                StudentName.Text = Edit.StudentName;
                StudentDateOfBirth.EditValue = Edit.StudentDateOfBirth.ToString();
                StudentAddress.Text = Edit.StudentAddress;
                StudentPhone.Text = Edit.StudentPhone;
                StudentJoiningDate.EditValue= Edit.StudentJoiningDate.ToString();
                StudentHealthStatus.Text= Edit.StudentHealthStatus;
                ClassId.EditValue = Edit.ClassId.ToString();
                StudentPicture.EditValue= Edit.StudentPicture;
                FatherName.Text = Edit.FatherName ;
                FatherAddress.Text= Edit.FatherAddress;
                FatherPhone.Text = Edit.FatherPhone;
                FatherJob.Text= Edit.FatherJob;
                FatherEmail.Text = Edit.FatherEmail;
                MotherName.Text= Edit.MotherName;
                MotherAddress.Text= Edit.MotherAddress;
                MotherJob.Text= Edit.MotherJob;
                MotherPhone.Text= Edit.MotherPhone;
                MotherEmail.Text= Edit.MotherEmail;
                EmergencyPersonName.Text= Edit.EmergencyPersonName;
                EmergencyPersonPhone.Text= Edit.EmergencyPersonPhone;
                RegisterFees.Text= Edit.RegisterFees;
                PaymentTypeId.EditValue=Edit.PaymentTypeId.ToString();
                Notes.Text= Edit.Notes;
                StudentBarCode.Text=Edit.StudentBarCode;
                DeductValue.Text = Edit.DeductValue;
                Age.Text = Edit.Age;
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            Clear();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            InsertOrUpdate();
        }

        string loadedPath;
        private void StudentPicture_DoubleClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp;*.png;*.ico)|*.jpg; *.jpeg; *.gif; *.bmp;*.png;*.ico";
            DialogResult dr = ofd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                Image loadedImage = Image.FromFile(ofd.FileName);
                loadedPath = ofd.FileName;
                StudentPicture.Image = loadedImage;
                StudentPicture.Tag = ofd.FileName;
            }
        }









    }
}