using Newtonsoft.Json;
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

namespace КурсоваяПрог.Интерфейс.Упр._обьектами
{
    public partial class RequestForm : Form
    {
        private List<Project> projects; // Список проектов
        private List<Request> requests; // Список заявок
        private Request currentRequest; // Текущая заявка
        private ConstructionMaterial currentEditingMaterial; // Для хранения редактируемого материала
        private DataManager dataManager; // Менеджер данных
        private string dataDirectory; // Путь к директории для сохранения данных
        private bool isUpdatingDataGridView; // Флаг для предотвращения повторного вызова

        public delegate void SaveRequestHandler(Request request);
        public event SaveRequestHandler SaveRequestEvent;



        // -----
        public delegate void ProjectNameUpdatedHandler(string oldProjectName, string newProjectName);
        public event ProjectNameUpdatedHandler ProjectNameUpdated;

        public RequestForm(string dataDirectory)
        {
            InitializeComponent(); // Инициализация компонентов формы
            this.dataDirectory = dataDirectory; // Установка пути к директории
            dataManager = new DataManager(); // Создание экземпляра DataManager

            requests = new List<Request>(); // Инициализация списка заявок
            currentRequest = new Request(); // Инициализация текущей заявки
            InitializeDataGridViews(); // Инициализация DataGridView
            LoadProjects(); // Загрузка проектов
            InitializeUnitComboBox(); // Инициализация ComboBox для единиц измерения

            // Подписка на событие изменения выбора
            listBoxProjects.SelectedIndexChanged += ListBoxProjects_SelectedIndexChanged; // Подписка на изменение выбора в списке проектов
        }


        // добавил 2 метода снизу
        public void UpdateRequests(string oldProjectName, string newProjectName)
        {
            requests = dataManager.LoadFromFile<List<Request>>(dataDirectory);
            foreach (var request in requests)
            {
                if (request.ProjectName == oldProjectName)
                {
                    request.ProjectName = newProjectName;
                }
            }
            dataManager.SaveToFile(dataDirectory, requests);
            MessageBox.Show("Имя проекта в заявках успешно обновлено!", "Успех", MessageBoxButtons.OK);
            LoadRequests();
        }




        // Метод для вызова события
        public void OnProjectNameUpdated(string oldProjectName, string newProjectName)
        {
            // Вызываем метод обновления напрямую
            UpdateRequests(oldProjectName, newProjectName);
        }

        private void ListBoxProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingDataGridView) return; // Проверка флага, чтобы избежать повторного вызова

            if (listBoxProjects.SelectedItem is Project selectedProject) // Если выбран проект
            {
                isUpdatingDataGridView = true; // Устанавливаем флаг

                // Очищаем предыдущие данные в таблице
                dataGridViewProjects.DataSource = null; // Убираем источник данных
                dataGridViewProjects.Rows.Clear(); // Очищаем строки

                // Создаем новую строку с нужными данными
                var newRow = new DataGridViewRow();
                newRow.CreateCells(dataGridViewProjects, selectedProject.Name, DateTime.Now); // Устанавливаем только нужные данные

                // Добавляем новую строку в таблицу
                dataGridViewProjects.Rows.Add(newRow);

                // Загружаем заявки для выбранного проекта
                LoadRequestsForProject(selectedProject);

                isUpdatingDataGridView = false; // Сбрасываем флаг
            }
        }

        private void LoadRequestsForProject(Project selectedProject)
        {
            var filePath = Path.Combine(dataDirectory, "requests.json"); // Путь к файлу с заявками
            if (File.Exists(filePath)) // Проверяем, существует ли файл
            {
                var jsonData = File.ReadAllText(filePath); // Читаем данные из файла
                requests = JsonConvert.DeserializeObject<List<Request>>(jsonData) ?? new List<Request>(); // Десериализуем данные в список заявок

                // Находим соответствующую заявку для выбранного проекта
                currentRequest = requests.FirstOrDefault(req => req.ProjectName == selectedProject.Name);

                // Если текущая заявка не инициализирована, создаем новую
                if (currentRequest == null)
                {
                    currentRequest = new Request
                    {
                        ProjectName = selectedProject.Name, // Устанавливаем имя проекта
                        RequestDate = DateTime.Now, // Устанавливаем текущую дату
                        OrderedMaterials = new List<ConstructionMaterial>() // Инициализация как BindingList
                    };
                }

                // Обновляем таблицу материалов для текущей заявки
                UpdateMaterialsGrid(currentRequest.OrderedMaterials); // Обновляем отображение материалов

                if (currentRequest != null) // Если заявка найдена
                {
                    MessageBox.Show("Заявка загружена для выбранного проекта.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information); // Сообщение об успешной загрузке
                                                                                                                                                 // Обновляем дату в таблице объектов
                    if (dataGridViewProjects.Rows.Count > 0) // Проверяем, есть ли строки в таблице
                    {
                        dataGridViewProjects.Rows[0].Cells["RequestDate"].Value = currentRequest.RequestDate; // Устанавливаем дату заявки
                    }
                }
                else // Если заявка не найдена
                {
                    MessageBox.Show("Заявка для выбранного проекта не найдена. Хотите создать новую заявку?", "Информация", MessageBoxButtons.YesNo);
                    if (DialogResult.Yes == MessageBox.Show("Хотите создать новую заявку?", "Информация", MessageBoxButtons.YesNo))
                    {
                        // Создаем новую заявку
                        currentRequest = new Request
                        {
                            ProjectName = selectedProject.Name, // Устанавливаем имя проекта
                            RequestDate = DateTime.Now, // Устанавливаем текущую дату
                            OrderedMaterials = new List<ConstructionMaterial>() // Инициализация как BindingList
                        };
                        UpdateMaterialsGrid(currentRequest.OrderedMaterials); // Обновляем таблицу материалов
                        MessageBox.Show("Новая заявка создана.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information); // Сообщение об успешном создании новой заявки
                    }
                    else
                    {
                        UpdateMaterialsGrid(null); // Очищаем таблицу материалов
                    }
                }
            }
            else
            {
                MessageBox.Show("Файл с заявками не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
            }
        }


        private void InitializeUnitComboBox()
        {
            comboBoxMaterialUnit.Items.AddRange(new[] { "кг", "тонна", "шт" });
            comboBoxMaterialUnit.SelectedIndex = 0; // Устанавливаем значение по умолчанию
        }

        private void DateTimePickerRequestDate_ValueChanged(object sender, EventArgs e) // Обработчик события изменения даты заявки
        {
            if (currentRequest != null) // Если текущая заявка не равна null
            {
                currentRequest.RequestDate = dateTimePickerRequestDate.Value; // Обновляем дату заявки

                // Обновляем дату в таблице объектов
                if (dataGridViewProjects.SelectedRows.Count > 0) // Если выбрана строка в таблице объектов
                {
                    var selectedRow = dataGridViewProjects.SelectedRows[0]; // Получаем выбранную строку
                    selectedRow.Cells["RequestDate"].Value = currentRequest.RequestDate; // Обновляем дату в таблице
                }
            }
        }


        private void LoadProjects()
        {
            var filePath = Path.Combine(dataDirectory, "projects.json"); // Путь к файлу с проектами
            if (File.Exists(filePath)) // Проверяем, существует ли файл
            {
                var jsonData = File.ReadAllText(filePath); // Читаем данные из файла
                projects = JsonConvert.DeserializeObject<List<Project>>(jsonData); // Десериализуем данные в список проектов
                if (projects != null && projects.Count > 0) // Проверяем, что проекты загружены
                {
                    // Заполняем ListBox
                    listBoxProjects.DataSource = projects; // Устанавливаем источник данных для ListBox
                    listBoxProjects.DisplayMember = "Name"; // Отображаем название проекта
                    listBoxProjects.ValueMember = "Id"; // Устанавливаем уникальный идентификатор

                    // Также заполняем DataGridView
                    dataGridViewProjects.DataSource = projects; // Устанавливаем источник данных для DataGridView
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить проекты.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
                }
            }
            else
            {
                MessageBox.Show("Файл с проектами не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
            }
        }

        private void UpdateMaterialsGrid(IEnumerable<ConstructionMaterial> materials) // Метод для обновления таблицы материалов
        {
            if (materials != null && materials.Any()) // Проверяем, есть ли материалы
            {
                dataGridViewMaterials.DataSource = materials.ToList(); // Устанавливаем новый источник данных
            }
            else
            {
                dataGridViewMaterials.DataSource = null; // Очистка источника данных
            }
        }


        private void InitializeDataGridViews()
        {
            // Настройка DataGridView для объектов
            dataGridViewProjects.AutoGenerateColumns = false;
            dataGridViewProjects.Columns.Clear();
            dataGridViewProjects.Columns.Add(new DataGridViewTextBoxColumn { Name = "ObjectName", HeaderText = "Название объекта", DataPropertyName = "Name" });
            dataGridViewProjects.Columns.Add(new DataGridViewTextBoxColumn { Name = "RequestDate", HeaderText = "Дата заявки", DataPropertyName = "RequestDate" }); // Добавляем столбец для даты заявки

            // Настройка DataGridView для материалов
            dataGridViewMaterials.AutoGenerateColumns = false;
            dataGridViewMaterials.Columns.Clear();
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaterialName", HeaderText = "Название материала", DataPropertyName = "Name" });
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Unit", HeaderText = "Единица измерения", DataPropertyName = "Unit" });
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Требуемое количество", DataPropertyName = "Quantity" });
        }

        private void LoadRequests()
        {
            var filePath = Path.Combine(dataDirectory, "requests.json");
            if (File.Exists(filePath))
            {
                var jsonData = File.ReadAllText(filePath);
                requests = JsonConvert.DeserializeObject<List<Request>>(jsonData) ?? new List<Request>();
                if (requests != null && requests.Count > 0)
                {
                    if (listBoxProjects.SelectedItem is Project selectedProject)
                    {
                        currentRequest = requests.FirstOrDefault(req => req.OrderedMaterials != null && req.OrderedMaterials.Any() && req.ProjectName == selectedProject.Name);
                    }
                    UpdateMaterialsGrid(currentRequest?.OrderedMaterials);
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить заявки.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Файл с заявками не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnDeleteRequest_Click(object sender, EventArgs e)
        {
            if (dataGridViewProjects.SelectedRows.Count == 1) // Проверяем, выбрана ли одна строка
            {
                var selectedRow = dataGridViewProjects.SelectedRows[0]; // Получаем выбранную строку
                var projectName = selectedRow.Cells["ConstructionObjectTitle"].Value.ToString(); // Получаем название объекта

                var requestToRemove = requests.FirstOrDefault(r => r.ProjectName == projectName); // Находим заявку для удаления
                if (requestToRemove != null) // Если заявка найдена
                {
                    requests.Remove(requestToRemove); // Удаляем заявку из списка
                    dataGridViewProjects.Rows.Remove(selectedRow); // Удаляем строку из таблицы
                    dataGridViewMaterials.Rows.Clear(); // Очищаем таблицу материалов
                    currentRequest = null; // Сбрасываем текущую заявку
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите одну заявку для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
            }
        }



        private void btnAddMaterial_Click(object sender, EventArgs e)
        {
            // Проверка на пустые поля
            if (string.IsNullOrWhiteSpace(textBoxMaterialName.Text) ||
                string.IsNullOrWhiteSpace(comboBoxMaterialUnit.SelectedItem?.ToString()) ||
                numericUpDownQuantity.Value <= 0) // Проверяем, заполнены ли все поля
            {
                MessageBox.Show("Пожалуйста, заполните все поля для материала.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Проверка, что название материала не состоит только из цифр
            if (textBoxMaterialName.Text.Trim().All(char.IsDigit))
            {
                MessageBox.Show("Название материала не может состоять только из цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var material = new ConstructionMaterial // Создаем новый материал
            {
                Name = textBoxMaterialName.Text.Trim(),
                Unit = comboBoxMaterialUnit.SelectedItem.ToString(),
                Quantity = (double)numericUpDownQuantity.Value
            };

            // Добавляем материал в текущую заявку
            if (currentRequest != null)
            {
                currentRequest.OrderedMaterials.Add(material); // Добавляем материал в список
                SaveRequest(currentRequest); // Сохраняем изменения в файл
                UpdateMaterialsGrid(currentRequest.OrderedMaterials); // Обновляем таблицу материалов
            }
            else
            {
                MessageBox.Show("Текущая заявка не инициализирована.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ClearMaterialInputFields(); // Очищаем поля ввода материала
        }



        private void ClearMaterialInputFields() // Метод для очистки полей ввода материала
        {
            textBoxMaterialName.Clear(); // Очищаем текстовое поле для названия материала
            comboBoxMaterialUnit.SelectedIndex = 0; // Сбрасываем выбор в ComboBox
            numericUpDownQuantity.Value = 0; // Сбрасываем значение NumericUpDown
        }



        private void ClearForm()
        {
            dateTimePickerRequestDate.Value = DateTime.Now; // Сброс даты
            dataGridViewProjects.DataSource = null; // Очищаем источник данных для таблицы объектов
            dataGridViewMaterials.DataSource = null; // Очищаем источник данных для таблицы материалов
            ClearMaterialInputFields(); // Очищаем поля ввода
        }



        

        private void dataGridViewMaterials_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewMaterials.SelectedRows.Count == 1) // Если выбрана одна строка
            {
                var selectedRow = dataGridViewMaterials.SelectedRows[0]; // Получаем выбранную строку
                if (selectedRow.Cells["MaterialName"].Value != null) // Проверяем, что значение не null
                {
                    textBoxMaterialName.Text = selectedRow.Cells["MaterialName"].Value.ToString(); // Устанавливаем название материала
                    comboBoxMaterialUnit.SelectedItem = selectedRow.Cells["Unit"].Value.ToString(); // Устанавливаем единицу измерения
                    numericUpDownQuantity.Value = Convert.ToDecimal(selectedRow.Cells["Quantity"].Value); // Устанавливаем количество
                }
            }
            else
            {
                ClearMaterialInputFields(); // Очищаем поля ввода, если выбрано больше одной строки
            }
        }

        private void btnSaveRequest_Click(object sender, EventArgs e)
        {
            if (listBoxProjects.SelectedItem is Project selectedProject)
            {
                // Сохранение заявки в файл
                SaveRequest(currentRequest);
                MessageBox.Show("Заявка успешно сохранена.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearForm(); // Очищаем форму после сохранения
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите проект.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        public void SaveRequest(Request request)
        {
            var requests = dataManager.LoadFromFile<List<Request>>(Path.Combine(dataDirectory, "requests.json")) ?? new List<Request>();

            // Проверяем, существует ли заявка для текущего проекта
            var existingRequest = requests.FirstOrDefault(req => req.ProjectName == request.ProjectName);

            if (existingRequest != null)
            {
                // Если заявка существует, обновляем ее
                existingRequest.RequestDate = request.RequestDate;
                existingRequest.OrderedMaterials = request.OrderedMaterials?.Any() == true ? request.OrderedMaterials : existingRequest.OrderedMaterials;
            }
            else
            {
                // Если заявки не существует, добавляем новую
                requests.Add(request);
            }

            var jsonData = JsonConvert.SerializeObject(requests, Formatting.Indented);
            File.WriteAllText(Path.Combine(dataDirectory, "requests.json"), jsonData);
        }

        private void btnDeleteMaterial_Click(object sender, EventArgs e)
        {
            if (dataGridViewMaterials.SelectedRows.Count == 1) // Проверяем, выбрана ли одна строка
            {
                var selectedRow = dataGridViewMaterials.SelectedRows[0]; // Получаем выбранную строку

                if (currentRequest == null) // Проверяем, инициализирована ли текущая заявка
                {
                    MessageBox.Show("Текущая заявка не инициализирована.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
                    return;
                }

                var materialToDelete = currentRequest.OrderedMaterials.FirstOrDefault(m => m.Name == selectedRow.Cells["MaterialName"].Value.ToString()); // Находим материал для удаления
                if (materialToDelete != null) // Если материал найден
                {
                    currentRequest.OrderedMaterials.Remove(materialToDelete); // Удаляем материал из заявки
                    SaveRequest(currentRequest); // Сохраняем изменения в файл
                    UpdateMaterialsGrid(currentRequest.OrderedMaterials); // Обновляем таблицу материалов
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите один материал для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
            }
        }





        private void btnEditMaterial_Click_1(object sender, EventArgs e)
        {
            if (dataGridViewMaterials.SelectedRows.Count == 1) // Проверяем, выбрана ли одна строка
            {
                if (currentRequest == null) // Проверяем, инициализирована ли текущая заявка
                {
                    MessageBox.Show("Текущая заявка не инициализирована.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
                    return;
                }

                var selectedRow = dataGridViewMaterials.SelectedRows[0]; // Получаем выбранную строку

                if (selectedRow.Cells["MaterialName"].Value == null || // Проверяем, что значения не null
                    selectedRow.Cells["Unit"].Value == null || // Изменено с "MaterialUnit" на "Unit"
                    selectedRow.Cells["Quantity"].Value == null) // Изменено с "MaterialQuantity" на "Quantity"
                {
                    MessageBox.Show("Ошибка: выбранный материал содержит недопустимые значения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
                    return;
                }

                textBoxMaterialName.Text = selectedRow.Cells["MaterialName"].Value.ToString(); // Устанавливаем название материала
                comboBoxMaterialUnit.SelectedItem = selectedRow.Cells["Unit"].Value.ToString(); // Устанавливаем единицу измерения
                numericUpDownQuantity.Value = Convert.ToDecimal(selectedRow.Cells["Quantity"].Value); // Устанавливаем количество
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите один материал для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
            }
        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (dataGridViewMaterials.SelectedRows.Count == 1) // Проверяем, выбрана ли одна строка
            {
                var selectedRow = dataGridViewMaterials.SelectedRows[0]; // Получаем выбранную строку

                if (currentRequest == null) // Проверяем, инициализирована ли текущая заявка
                {
                    MessageBox.Show("Текущая заявка не инициализирована.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
                    return;
                }

                // Проверка на пустые поля
                if (string.IsNullOrWhiteSpace(textBoxMaterialName.Text) ||
                    string.IsNullOrWhiteSpace(comboBoxMaterialUnit.SelectedItem?.ToString()) ||
                    numericUpDownQuantity.Value <= 0) // Проверяем, заполнены ли все поля
                {
                    MessageBox.Show("Пожалуйста, заполните все поля для материала.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
                    return;
                }

                // Проверка, что название материала не состоит только из цифр
                if (textBoxMaterialName.Text.Trim().All(char.IsDigit))
                {
                    MessageBox.Show("Название материала не может состоять только из цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
                    return;
                }

                var materialToUpdate = currentRequest.OrderedMaterials.FirstOrDefault(m => m.Name == selectedRow.Cells["MaterialName"].Value.ToString()); // Находим материал для обновления
                if (materialToUpdate != null) // Если материал найден
                {
                    materialToUpdate.Name = textBoxMaterialName.Text.Trim(); // Убираем пробелы
                    materialToUpdate.Unit = comboBoxMaterialUnit.SelectedItem.ToString(); // Обновляем единицу измерения
                    materialToUpdate.Quantity = (double)numericUpDownQuantity.Value; // Обновляем количество

                    // Обновляем значения в таблице
                    selectedRow.Cells["MaterialName"].Value = materialToUpdate.Name;
                    selectedRow.Cells["Unit"].Value = materialToUpdate.Unit;
                    selectedRow.Cells["Quantity"].Value = materialToUpdate.Quantity;

                    SaveRequest(currentRequest); // Сохраняем изменения в файл
                    UpdateMaterialsGrid(currentRequest.OrderedMaterials); // Обновляем таблицу материалов
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите один материал для сохранения изменений.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
            }
        }




        private void textBoxMaterialName_TextChanged(object sender, EventArgs e)
        {

        }
        private void comboBoxMaterialUnit_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void numericUpDownQuantity_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}