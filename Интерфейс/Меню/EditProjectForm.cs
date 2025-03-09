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
using Newtonsoft.Json;
using КурсоваяПрог.Интерфейс.Упр._обьектами;
using static КурсоваяПрог.Интерфейс.Упр._обьектами.RequestForm;



namespace КурсоваяПрог.Интерфейс
{
    public partial class EditProjectForm : Form
    {
        public Project UpdatedProject { get; private set; }
        private DataManager dataManager = new DataManager(); // Создаем экземпляр DataManager
        private const string DataDirectoryPath = "Data"; // Путь к директории для сохранения данных

        // ---
        private RequestForm requestForm;

        public EditProjectForm(Project project)
        {
            InitializeComponent();
        

            txtName.Text = project.Name;
            txtAddress.Text = project.Address;
            InitializeStatusComboBox(); // Инициализация ComboBox для статусов
            comboBoxStatus.SelectedItem = project.Status; // Устанавливаем выбранный статус
            dateTimePickerStart.Value = project.StartDate; // Установка даты начала
            dateTimePickerEnd.Value = project.EndDate;     // Установка даты окончания
            UpdatedProject = project;
        }


        private void InitializeStatusComboBox()
        {
            comboBoxStatus.Items.Add("В процессе");
            comboBoxStatus.Items.Add("Завершен");
            comboBoxStatus.Items.Add("Отменен");
            comboBoxStatus.SelectedIndex = 0; // Устанавливаем значение по умолчанию
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            // Проверка ввода
            if (!ValidateInput())
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Прерываем выполнение, если ввод некорректен
            }

            // Сохраняем старое имя проекта для обновления заявок
            string oldProjectName = UpdatedProject.Name;

            // Обновление свойств проекта
            UpdatedProject.Name = txtName.Text.Trim(); // Убираем пробелы в начале и конце
            UpdatedProject.Address = txtAddress.Text.Trim(); // Убираем пробелы в начале и конце
            UpdatedProject.Status = comboBoxStatus.SelectedItem.ToString(); // Получаем выбранный статус из ComboBox
            UpdatedProject.StartDate = dateTimePickerStart.Value; // Обновление даты начала
            UpdatedProject.EndDate = dateTimePickerEnd.Value;     // Обновление даты окончания

            // Обновляем имя проекта в смете
            UpdateEstimateProjectName(oldProjectName, UpdatedProject.Name);


           

            DialogResult = DialogResult.OK;
            Close();
        }



        private bool ValidateInput()
        {
            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Название проекта не может быть пустым или содержать только пробелы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; // Одно из полей пустое
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Адрес проекта не может быть пустым или содержать только пробелы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; // Одно из полей пустое
            }

            if (comboBoxStatus.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите статус проекта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; // Статус не выбран
            }

            // Проверка на наличие пробелов в названии и адресе
            if (txtName.Text.Trim().Length == 0)
            {
                MessageBox.Show("Название проекта не может состоять только из пробелов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (txtAddress.Text.Trim().Length == 0)
            {
                MessageBox.Show("Адрес проекта не может состоять только из пробелов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Проверка на корректность дат
            if (dateTimePickerStart.Value >= dateTimePickerEnd.Value)
            {
                MessageBox.Show("Дата начала должна быть раньше даты окончания.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false; // Дата начала должна быть раньше даты окончания
            }

            return true; // Все проверки пройдены
        }


        private void UpdateEstimateProjectName(string oldProjectName, string newProjectName)
        {
            var estimatesPath = Path.Combine(DataDirectoryPath, "estimates.json");
            var estimates = dataManager.LoadFromFile<List<Estimate>>(estimatesPath) ?? new List<Estimate>();

            // Находим смету, соответствующую старому имени проекта
            var existingEstimate = estimates.FirstOrDefault(est => est.ProjectName == oldProjectName);

            if (existingEstimate != null)
            {
                // Если смета найдена, обновляем имя проекта
                existingEstimate.ProjectName = newProjectName;

                // Сохраняем изменения в файл
                var jsonData = JsonConvert.SerializeObject(estimates, Formatting.Indented);
                File.WriteAllText(estimatesPath, jsonData);
            }
        }
        





        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
