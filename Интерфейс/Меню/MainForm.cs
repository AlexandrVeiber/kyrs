using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using КурсоваяПрог.Интерфейс;
using System.IO;
using КурсоваяПрог.Интерфейс.Бригады;
using КурсоваяПрог.Интерфейс.склад;
using КурсоваяПрог.Интерфейс.Упр._обьектами;


namespace КурсоваяПрог
{
    public partial class MainForm : Form
    {
        private List<Request> requests; // Список заявок
        private List<Project> projects = new List<Project>();
        private const string DataDirectoryPath = "Data"; // Путь к директории для сохранения данных
        private DataManager dataManager = new DataManager(); // Создаем экземпляр DataManager
        private List<ConstructionMaterial> materials = new List<ConstructionMaterial>();



        public MainForm()
        {
            InitializeComponent();
            LoadProjects(); // Загружаем проекты из файла при инициализации
        }



        private void MainForm_Load(object sender, EventArgs e)
        {
            UpdateDataSources();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveProjects(); // Сохраняем проекты перед закрытием формы
        }

        private void btnAddProject_Click(object sender, EventArgs e)
        {
            using (var addProjectForm = new AddProjectForm()) // Создаем форму для добавления проекта
            {
                if (addProjectForm.ShowDialog() == DialogResult.OK) // Проверяем, был ли результат "ОК"
                {
                    var newProject = addProjectForm.NewProject; // Получаем новый проект
                    projects.Add(newProject); // Добавляем проект в список
                    UpdateDataSources(); // Обновляем источники данных
                }
            }
        }

        private void btnEditProject_Click(object sender, EventArgs e)
        {
            if (listBoxObjects.SelectedItem != null) // Проверяем, выбран ли проект
            {
                var selectedProject = listBoxObjects.SelectedItem as Project; // Получаем выбранный проект

                // Создаем экземпляр EditProjectForm
                using (var editProjectForm = new EditProjectForm(selectedProject))
                {
                    if (editProjectForm.ShowDialog() == DialogResult.OK) // Проверяем, был ли результат "ОК"
                    {
                        // Обновляем проект в списке
                        int selectedIndex = projects.IndexOf(selectedProject); // Получаем индекс выбранного проекта
                        projects[selectedIndex] = editProjectForm.UpdatedProject; // Обновляем проект в списке

                        // Сохраняем изменения в файл
                        SaveProjects(); // Сохраняем проекты

                        UpdateDataSources(); // Обновляем источники данных
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите проект для редактирования."); // Сообщение об ошибке
            }
        }
        private void btnDeleteProject_Click(object sender, EventArgs e)
        {
            if (listBoxObjects.SelectedItem != null) // Проверяем, выбран ли проект
            {
                var selectedProject = listBoxObjects.SelectedItem as Project; // Получаем выбранный проект
                projects.Remove(selectedProject); // Удаляем проект из списка
                SaveProjects(); // Сохраняем изменения в файл
                UpdateDataSources(); // Обновляем отображение
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите проект для удаления."); // Сообщение об ошибке
            }
        }


        public void LoadProjects() // Метод для загрузки проектов из файла
        {
            if (Directory.Exists(DataDirectoryPath)) // Проверяем, существует ли директория данных
            {
                // Загружаем проекты и материалы
                var (loadedProjects, loadedBuilders, loadedCompletedWorks, loadedDeliveries, loadedMaterials, loadedRequests) = dataManager.LoadAllData(DataDirectoryPath);
                projects = loadedProjects; // Загружаем проекты
                materials = loadedMaterials; // Инициализация переменной materials
                requests = loadedRequests; // Инициализация перем енной requests
                UpdateDataSources(); // Обновляем источники данных
            }
            else
            {
                MessageBox.Show("Директория данных не найдена."); // Сообщение об ошибке
            }
        }

        private void SaveProjects() // Метод для сохранения проектов в файл
        {
            dataManager.SaveAllData(DataDirectoryPath, projects, new List<Builder>(), new List<CompletedWork>(), new List<Delivery>(), materials, requests); // Сохраняем проекты
        }



        private void dataGridViewProjects_CellContentClick(object sender, DataGridViewCellEventArgs e) // Обработчик события нажатия на ячейку в DataGridView
        {
            if (e.RowIndex >= 0) // Проверяем, что индекс строки корректен
            {
                // Получение измененного значения
                var newValue = dataGridViewProjects.Rows[e.RowIndex].Cells[e.ColumnIndex].Value; // Получаем новое значение из ячейки

                // Обновление объекта в списке
                var project = projects[e.RowIndex]; // Получаем проект по индексу
                switch (e.ColumnIndex) // Проверяем индекс столбца
                {
                    case 1: // Название
                        project.Name = newValue.ToString(); // Обновляем имя проекта
                        break;
                    case 2: // Адрес
                        project.Address = newValue.ToString(); // Обновляем адрес проекта
                        break;
                    case 3: // Статус
                        project.Status = newValue.ToString(); // Обновляем статус проекта
                        break;
                }
            }
        }

        private void btnSelect_Click(object sender, EventArgs e) // Обработчик события нажатия кнопки "Выбрать"
        {
            if (listBoxObjects.SelectedItem != null) // Проверяем, выбран ли проект
            {
                var selectedProject = listBoxObjects.SelectedItem as Project; // Получаем выбранный проект
                if (selectedProject != null) // Проверяем, что проект не равен null
                {
                    MessageBox.Show($"Вы выбрали: {selectedProject.Name}\nАдрес: {selectedProject.Address}\nСтатус: {selectedProject.Status}"); // Выводим информацию о проекте
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите проект из списка."); // Сообщение об ошибке
            }
        }

        private void UpdateDataSources() // Метод для обновления источников данных
        {
            dataGridViewProjects.DataSource = null; // Сбрасываем источник данных
            dataGridViewProjects.DataSource = projects; // Устанавливаем новый источник данных
            listBoxObjects.DataSource = null; // Сбрасываем источник данных для списка
            listBoxObjects.DataSource = projects; // Устанавливаем новый источник данных для списка
            listBoxObjects.DisplayMember = "Name"; // Указываем, какое свойство отображать
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void добавитьБригадуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBoxObjects.SelectedItem != null) // Проверяем, выбран ли проект
            {
                var selectedProject = listBoxObjects.SelectedItem as Project; // Получаем выбранный проект
                using (var brigadeForm = new BrigadeForm(selectedProject, projects)) // Передаем выбранный проект и список проектов
                {
                    brigadeForm.ShowDialog(); // Открываем форму для добавления бригады
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите проект, в который хотите добавить бригаду."); // Сообщение об ошибке
            }
        }

        private void добавитьПоставщикаToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void складToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Создаем экземпляр формы "Склад"
            WarehouseForm warehouseForm = new WarehouseForm();

            // Открываем форму "Склад"
            warehouseForm.Show(); // Используйте ShowDialog(), если хотите, чтобы форма была модальной
        }

        private void сметаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Убедитесь, что у вас есть путь к директории данных
            string dataDirectory = @"C:\Users\kansvet\source\repos\КурсоваяПрог\КурсоваяПрог\bin\Debug\Data"; // Замените на ваш путь к директории

            // Создание экземпляра формы "Смета"
            EstimateForm estimateForm = new EstimateForm(dataDirectory);

            // Открываем форму "Смета"
            estimateForm.Show(); // Используйте ShowDialog(), если хотите, чтобы форма была модальной
        }

        private void заявкиToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string dataDirectory = @"C:\Users\kansvet\source\repos\КурсоваяПрог\КурсоваяПрог\bin\Debug\Data"; // Замените на ваш путь к директории


            // Создаем экземпляр формы "RequestForm"
            RequestForm requestForm = new RequestForm(dataDirectory); // Передаем путь к директории данных

            // Открываем форму "RequestForm"
            requestForm.Show(); // Используйте ShowDialog(), если хотите, чтобы форма была модальной
        }

        private void работыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // Создаем экземпляр формы WorkRecordForm
            WorkRecordForm workRecordForm = new WorkRecordForm();

            // Открываем форму
            workRecordForm.Show(); // Используйте ShowDialog(), если хотите, чтобы форма была модальной
        }
    }
}
