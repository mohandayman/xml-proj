using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;

namespace XML_File_Crud_Operations
{
    public partial class CrudForm : Form
    {
        // fields
        private string _xmlFilePath;
        private List<Employee> _employees;

        private int _employeeNavigator;

        public int EmployeeNavigator
        {
            get { return _employeeNavigator; }
            set { _employeeNavigator = value; }
        }


        private XmlDocument xmlDoc;
        // constructor
        public CrudForm()
        {
            
            InitializeComponent();

            // initialize document and load
            _xmlFilePath = "Employees.xml";
            xmlDoc = new XmlDocument();
            xmlDoc.Load(_xmlFilePath);

            // preparing employees list
            _employees = new List<Employee>();
            EmployeeNavigator = 0;


            ReadEmpFromFile(0);
        }




        // Methods
        private static string SerializeToXmlString(Employee emp)
        {
            var result = "";
            var xmlSerializer = new XmlSerializer(emp.GetType());
            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw, new XmlWriterSettings { Indent = false }))
                {
                    xmlSerializer.Serialize(writer, emp);
                    result = sw.ToString();
                }
            }
            return result;
        }

        private Employee TakeEmployeeData()
        {
            Employee employee = new Employee();
            employee.Name = NameTextBox.Text;
            employee.Email = EmailTextBox.Text;
            employee.Address = AddressTextBox.Text;
            employee.Phone = phoneTextBox.Text;

            return employee;
        }

        private bool IsValidInputs()
        {
            bool isValid = false;
            if (
                NameTextBox.Text != string.Empty &&
                phoneTextBox.Text != string.Empty &&
                AddressTextBox.Text != string.Empty &&
                EmailTextBox.Text != string.Empty
              ) { isValid = true; }

            return isValid;
        }

        private void ReloadXmlDoc()
        {
            // todo: move those to end of save file
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(_xmlFilePath);
        }

        private void InsertEmpToXmlFile(Employee emp)
        {
            
            // selecting root
            XmlElement root = xmlDoc.DocumentElement;
            // creating elements
            XmlElement employee = xmlDoc.CreateElement("Employee");

            XmlElement name = xmlDoc.CreateElement("Name");
            name.InnerText = emp.Name;

            XmlElement phone = xmlDoc.CreateElement("Phone");
            phone.InnerText = emp.Phone;

            XmlElement address = xmlDoc.CreateElement("Address");
            address.InnerText = emp.Address;

            XmlElement email = xmlDoc.CreateElement("Email");
            email.InnerText = emp.Email;

            // append childs
            root.AppendChild(employee);
            employee.AppendChild(name);
            employee.AppendChild(phone);
            employee.AppendChild(address);
            employee.AppendChild(email);
        }

        private void ClearEmpsFromFile()
        {
            XmlElement root = xmlDoc.DocumentElement;
            root.RemoveAll();
        }
        private void SaveAllEmpsToXmlFile()
        {
            // todo: clear all employees in file
            ClearEmpsFromFile();
            // appending all employees
            foreach (Employee emp in _employees)
            {
                InsertEmpToXmlFile(emp);
            }
            // save document
            xmlDoc.Save(_xmlFilePath);
            // reload document
            ReloadXmlDoc();
        }


        private void ReadEmpFromFile(int index)
        {

            
            // selecting root
            XmlElement root = xmlDoc.DocumentElement;

            // getting childs and loop over them
            //XmlNodeList employeeNodes = root.ChildNodes;
            XmlNodeList empNodes = xmlDoc.SelectNodes("Company/Employee");
            for (int i=0; i<empNodes.Count;i++)
            {
                Employee emp = new Employee();
                //string test = employeeNodes[i].SelectSingleNode("/Name/").InnerText;
                emp.Name = empNodes[i].ChildNodes[0].InnerText;
                emp.Phone = empNodes[i].ChildNodes[1].InnerText;
                emp.Address = empNodes[i].ChildNodes[2].InnerText;
                emp.Email = empNodes[i].ChildNodes[3].InnerText;
                // save employees
                _employees.Add( emp );
            }
        }


        private void DisplayEmpData(int index)
        {
            
            if(index >= 0 && index < _employees.Count)
            {
                NameTextBox.Text = _employees[index].Name;
                phoneTextBox.Text = _employees[index].Phone;
                AddressTextBox.Text = _employees[index].Address;
                EmailTextBox.Text = _employees[index].Email;
                EmployeeNavigator = index;
            }
            else if (_employees.Count == 0)
            {
                // TODO: fire event of empty list
            }
        }


        // Event Handlers
        private void CrudForm_Load(object sender, EventArgs e)
        {
            // TODO: remove this
            // open or create xml file
            using (StreamWriter w = File.AppendText("Employees.xml"));

            // display first employee
            DisplayEmpData(EmployeeNavigator);
        }

        private void PreviousButton_Click(object sender, EventArgs e)
        {
            if(EmployeeNavigator > 0)
            {
                EmployeeNavigator--;
                DisplayEmpData(EmployeeNavigator);
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            if (EmployeeNavigator < _employees.Count -1)
            {
                EmployeeNavigator++;
                DisplayEmpData(EmployeeNavigator);
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string email = EmailTextBox.Text;
            for(int i=0; i< _employees.Count; i++)
            {
                if (_employees[i].Email == email)
                {
                    DisplayEmpData(i); break;
                }
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            _employees[EmployeeNavigator] = TakeEmployeeData();
        }

        private void InsertButton_Click(object sender, EventArgs e)
        {
            if (IsValidInputs())
            {
                Employee employee = TakeEmployeeData();
                _employees.Add(employee);
            }
        }

        

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            // remove current obj
            _employees.RemoveAt(EmployeeNavigator);
            // update navigator
            bool isLast = EmployeeNavigator == _employees.Count - 1;
            bool isFirst = EmployeeNavigator == 0;
            if (isFirst&&isLast)
            {
                // TODO: fire event of empty list
                //_employeesNavigator++;
            }

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveAllEmpsToXmlFile();
        }
    }
}
