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


namespace КурсоваяПрог.Интерфейс
{
    public partial class AddProjectForm : Form
    {
        public Project NewProject { get; private set; }
        private List<Project> projects = new List<Project>();
        private DataManager dataManager = new DataManager(); // Создаем экземпляр DataManager
        private const string DataDirectoryPath = "Data"; // Путь к директории для сохранения данных
        private List<ConstructionMaterial> materials = new List<ConstructionMaterial>(); // Объявление переменной
        public AddProjectForm()
        {
            InitializeComponent();
            LoadProjectsFromFile(); // Загружаем проекты при инициализации формы
            InitializeStatusComboBox(); // Инициализация ComboBox для статусов

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

            // Создание нового проекта
            NewProject = new Project // Инициализация нового объекта Project
            {
                Id = GenerateNewId(), // Генерация нового уникального идентификатора для проекта
                Name = txtName.Text, // Установка имени проекта из текстового поля
                Address = txtAddress.Text, // Установка адреса проекта из текстового поля
                Status = comboBoxStatus.SelectedItem.ToString(), // Получаем выбранный статус из ComboBox
                StartDate = dateTimePickerStart.Value, // Установка даты начала проекта из DateTimePicker
                EndDate = dateTimePickerEnd.Value // Установка даты окончания проекта из DateTimePicker
            };

            // Добавляем новый проект в список
            projects.Add(NewProject);

            // Сохраняем все проекты в файл
            dataManager.SaveAllData(DataDirectoryPath, projects, new List<Builder>(), new List<CompletedWork>(), new List<Delivery>(), materials, new List<Request>()); // Передаем пустой список для requests

            // Уведомляем главную форму о добавлении нового проекта
            (Owner as MainForm)?.LoadProjects(); // Обновляем проекты в главной форме

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

            // Проверка на наличие только пробелов в названии и адресе
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


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }


        private int GenerateNewId()
        {
            // Генерация нового ID на основе существующих проектов
            if (projects == null || projects.Count == 0)
            {
                return 1; // Если нет проектов, начинаем с 1
            }
            return projects.Max(p => p.Id) + 1; // Увеличиваем максимальный ID на 1
        }

        private void LoadProjectsFromFile()
        {
            if (Directory.Exists(DataDirectoryPath))
            {
                var (loadedProjects, _, _, _, loadedMaterials, loadedRequests) = dataManager.LoadAllData(DataDirectoryPath); // Загружаем проекты и материалы
                projects = loadedProjects ?? new List<Project>(); // Сохраняем загруженные проекты или создаем новый список
                                                                  // materials = loadedMaterials; // Если у вас есть переменная materials, инициализируйте ее здесь
            }
            else
            {
                projects = new List<Project>(); // Если директория не найдена, инициализируем пустой список
            }
        }
    }
}