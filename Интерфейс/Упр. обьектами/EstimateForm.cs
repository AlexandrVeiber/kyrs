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
using КурсоваяПрог.Интерфейс.склад;
using КурсоваяПрог.Интерфейс.склад.запасы_стройматериалов;

namespace КурсоваяПрог.Интерфейс.Упр._обьектами
{
    public partial class EstimateForm : Form
    {
        private List<Project> projects; // Список проектов
        private List<Estimate> estimates; // Список смет
        private Estimate currentEstimate; // Текущая смета
        private ConstructionMaterial currentEditingMaterial; // Для хранения редактируемого материала
        private DataManager dataManager; // Менеджер данных
        private string dataDirectory; // Путь к директории для сохранения данных
        private bool isUpdatingDataGridView; // Флаг для предотвращения повторного вызова

        public EstimateForm(string dataDirectory)
        {
            InitializeComponent(); // Инициализация компонентов формы
            this.dataDirectory = dataDirectory; // Установка пути к директории
            dataManager = new DataManager(); // Создание экземпляра DataManager
            estimates = new List<Estimate>(); // Инициализация списка смет
            currentEstimate = new Estimate(); // Инициализация текущей сметы
            InitializeDataGridViews(); // Инициализация DataGridView
            LoadProjects(); // Загрузка проектов
            LoadEstimates(); // Загрузка смет
            InitializeUnitComboBox(); // Инициализация ComboBox для единиц измерения

            // Подписка на событие изменения выбора
            listBoxProjects.SelectedIndexChanged += ListBoxProjects_SelectedIndexChanged; // Подписка на изменение выбора в списке проектов
            buttonSaveEditedMaterial.Click += buttonSaveEditedMaterial_Click; // Подписка на кнопку сохранения изменений
        }


        private void InitializeUnitComboBox()
        {
            comboBoxUnit.Items.Add("кг");
            comboBoxUnit.Items.Add("тонна");
            comboBoxUnit.Items.Add("шт"); // Добавьте другие единицы измерения по необходимости
            comboBoxUnit.SelectedIndex = 0; // Устанавливаем значение по умолчанию
        }


        private void ListBoxProjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingDataGridView) return; // Проверка флага, чтобы избежать повторного вызова

            if (listBoxProjects.SelectedItem is Project selectedProject) // Если выбран проект
            {
                isUpdatingDataGridView = true; // Устанавливаем флаг

                // Обновляем DataGridView для отображения выбранного проекта
                dataGridViewProjects.DataSource = new List<Project> { selectedProject }; // Устанавливаем источник данных

                // Проверяем, что estimates не равен null и содержит элементы
                if (estimates != null && estimates.Count > 0)
                {
                    // Находим соответствующую смету для выбранного проекта
                    currentEstimate = estimates.FirstOrDefault(est =>
                        est.ProjectName == selectedProject.Name); // Проверяем имя проекта

                    if (currentEstimate != null) // Если смета найдена
                    {
                        // Обновляем таблицу материалов
                        UpdateMaterialsGrid(currentEstimate.Materials); // Обновляем отображение материалов
                        MessageBox.Show("Смета загружена для выбранного проекта.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information); // Сообщение об успешной загрузке
                    }
                    else // Если смета не найдена
                    {
                        // Предлагаем создать новую смету
                        var result = MessageBox.Show("Смета для выбранного проекта не найдена. Хотите создать новую смету?", "Информация", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes) // Если пользователь согласен
                        {
                            // Создаем новую смету
                            currentEstimate = new Estimate
                            {
                                ProjectName = selectedProject.Name, // Устанавливаем имя проекта
                                Materials = new List<ConstructionMaterial>() // Инициализируем список материалов
                            };
                            UpdateMaterialsGrid(currentEstimate.Materials); // Обновляем таблицу материалов
                            MessageBox.Show("Новая смета создана.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information); // Сообщение об успешном создании новой сметы
                        }
                        else
                        {
                            UpdateMaterialsGrid(null); // Очищаем таблицу материалов
                        }
                    }
                }
                else // Если estimates пуст
                {
                    // Уведомляем пользователя и предлагаем создать новую смету
                    var result = MessageBox.Show("Файл смет пуст. Хотите создать новую смету для выбранного проекта?", "Информация", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes) // Если пользователь согласен
                    {
                        // Создаем новую смету
                        currentEstimate = new Estimate
                        {
                            ProjectName = selectedProject.Name, // Устанавливаем имя проекта
                            Materials = new List<ConstructionMaterial>() // Инициализируем список материалов
                        };
                        UpdateMaterialsGrid(currentEstimate.Materials); // Обновляем таблицу материалов
                        MessageBox.Show("Новая смета создана.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information); // Сообщение об успешном создании новой сметы
                    }
                    else
                    {
                        UpdateMaterialsGrid(null); // Очищаем таблицу материалов
                    }
                }

                // Обновляем имя проекта в смете
                UpdateEstimateProjectName(selectedProject.Name); // Обновляем имя проекта в смете

                isUpdatingDataGridView = false; // Сбрасываем флаг
            }
        }




        private void InitializeDataGridViews()
        {
            // Настройка DataGridView для объектов
            dataGridViewProjects.AutoGenerateColumns = false;
            dataGridViewProjects.Columns.Clear();
            dataGridViewProjects.Columns.Add(new DataGridViewTextBoxColumn { Name = "ObjectName", HeaderText = "Название объекта", DataPropertyName = "Name" });
            dataGridViewProjects.Columns.Add(new DataGridViewTextBoxColumn { Name = "ObjectAddress", HeaderText = "Адрес объекта", DataPropertyName = "Address" });

            // Настройка DataGridView для материалов
            dataGridViewMaterials.AutoGenerateColumns = false;
            dataGridViewMaterials.Columns.Clear();
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "MaterialName", HeaderText = "Название материала", DataPropertyName = "Name" });
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Unit", HeaderText = "Единица измерения", DataPropertyName = "Unit" });
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Требуемое количество", DataPropertyName = "Quantity" });
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

        private void LoadEstimates()
        {
            var filePath = Path.Combine(dataDirectory, "estimates.json"); // Путь к файлу со сметами
            if (File.Exists(filePath)) // Проверяем, существует ли файл
            {
                var jsonData = File.ReadAllText(filePath); // Читаем данные из файла
                estimates = JsonConvert.DeserializeObject<List<Estimate>>(jsonData); // Десериализуем данные в список смет
                if (estimates != null && estimates.Count > 0) // Проверяем, что сметы загружены
                {
                    // Устанавливаем текущую смету, если она существует
                    if (listBoxProjects.SelectedItem is Project selectedProject) // Проверяем, выбран ли проект
                    {
                        currentEstimate = estimates.FirstOrDefault(est =>
                            est.Materials != null && // Проверка на null
                            est.Materials.Any() && // Проверка на наличие материалов
                            est.ProjectName == selectedProject.Name); // Проверяем имя проекта
                    }

                    // Обновляем таблицу материалов для текущей сметы
                    UpdateMaterialsGrid(currentEstimate?.Materials); // Обновляем отображение материалов
                }
                else
                {
                    MessageBox.Show("Не удалось загрузить сметы.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
                }
            }
            else
            {
                MessageBox.Show("Файл с сметами не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
            }
        }


        private void buttonAddMaterial_Click(object sender, EventArgs e)
        {
            // Проверяем, что выбран проект
            if (listBoxProjects.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите проект из списка перед добавлением материалов.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверяем, что currentEstimate не равен null
            if (currentEstimate == null)
            {
                MessageBox.Show("Сначала выберите проект и создайте смету.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Убедитесь, что список материалов инициализирован
            if (currentEstimate.Materials == null)
            {
                currentEstimate.Materials = new List<ConstructionMaterial>();
            }

            // Получаем данные из текстовых полей
            string materialName = textBoxMaterialName.Text.Trim(); // Получаем название материала
            string unit = comboBoxUnit.SelectedItem?.ToString(); // Получаем выбранную единицу из ComboBox
            int quantity = (int)numericUpDownQuantity.Value; // Получаем значение из NumericUpDown

            // Проверка на пустые поля
            if (string.IsNullOrEmpty(materialName) || string.IsNullOrEmpty(unit))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на количество
            if (quantity <= 0)
            {
                MessageBox.Show("Количество должно быть больше нуля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка, что название материала не состоит только из цифр
            if (materialName.All(char.IsDigit))
            {
                MessageBox.Show("Название материала не может состоять только из цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверка на дубликаты
            if (currentEstimate.Materials.Any(m => m.Name.Equals(materialName, StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Материал с таким названием уже добавлен.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Создаем новый материал
            var newMaterial = new ConstructionMaterial
            {
                Name = materialName,
                Unit = unit,
                Quantity = quantity
            };

            // Добавляем новый материал к текущей смете
            currentEstimate.Materials.Add(newMaterial);

            // Обновляем список материалов для отображения
            UpdateMaterialsGrid(currentEstimate.Materials); // Обновляем таблицу материалов
            ClearMaterialInputFields(); // Очищаем текстовые поля

            MessageBox.Show("Материал успешно добавлен.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void buttonEditMaterial_Click(object sender, EventArgs e)
        {
            if (dataGridViewMaterials.SelectedRows.Count > 0)
            {
                var selectedMaterial = (ConstructionMaterial)dataGridViewMaterials.SelectedRows[0].DataBoundItem;

                // Заполняем поля ввода текущими значениями материала
                textBoxMaterialName.Text = selectedMaterial.Name;
                comboBoxUnit.SelectedItem = selectedMaterial.Unit; // Устанавливаем выбранную единицу измерения
                numericUpDownQuantity.Value = (decimal)selectedMaterial.Quantity; // Приведение к decimal

                // Сохраняем ссылку на редактируемый материал
                currentEditingMaterial = selectedMaterial; // Добавьте это поле в класс
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите материал для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonDeleteMaterial_Click(object sender, EventArgs e)
        {
            if (dataGridViewMaterials.SelectedRows.Count > 0)
            {
                var selectedMaterial = (ConstructionMaterial)dataGridViewMaterials.SelectedRows[0].DataBoundItem;
                currentEstimate.Materials.Remove(selectedMaterial); // Удаляем материал из объекта
                UpdateMaterialsGrid(currentEstimate.Materials); // Обновляем таблицу материалов
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите материал для удаления.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonSaveEstimate_Click(object sender, EventArgs e)
        {
            if (listBoxProjects.SelectedItem is Project selectedProject)
            {
                // Сохранение сметы в файл
                SaveEstimate(currentEstimate);
                MessageBox.Show("Смета успешно сохранена.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите проект.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void SaveEstimate(Estimate estimate)
        {
            var estimates = dataManager.LoadFromFile<List<Estimate>>(Path.Combine(dataDirectory, "estimates.json")) ?? new List<Estimate>();

            // Проверяем, существует ли смета для текущего проекта
            var existingEstimate = estimates.FirstOrDefault(est =>
                est.ProjectName == estimate.ProjectName); // Проверяем только имя проекта

            if (existingEstimate != null)
            {
                // Если смета существует, обновляем ее
                existingEstimate.Materials = estimate.Materials;
            }
            else
            {
                // Если смета не существует, добавляем новую
                estimates.Add(estimate);
            }

            var jsonData = JsonConvert.SerializeObject(estimates, Formatting.Indented);
            File.WriteAllText(Path.Combine(dataDirectory, "estimates.json"), jsonData);
        }

        private void UpdateEstimateProjectName(string newProjectName)
        {
            if (currentEstimate != null)
            {
                currentEstimate.ProjectName = newProjectName; // Обновляем имя проекта в текущей смете
                SaveEstimate(currentEstimate); // Сохраняем изменения в смете
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {

        }


        private void ClearMaterialInputFields()
        {
            textBoxMaterialName.Clear();
            comboBoxUnit.SelectedIndex = 0; // Сбрасываем выбор в ComboBox
            numericUpDownQuantity.Value = 0; // Сбрасываем значение NumericUpDown
        }

        private void buttonSaveEditedMaterial_Click(object sender, EventArgs e)
        {
            if (currentEditingMaterial != null)
            {
                // Получаем данные из текстовых полей
                string materialName = textBoxMaterialName.Text.Trim();
                string unit = comboBoxUnit.SelectedItem?.ToString(); // Получаем выбранную единицу из ComboBox
                int quantity = (int)numericUpDownQuantity.Value; // Получаем значение из NumericUpDown

                // Проверка на пустые поля
                if (string.IsNullOrEmpty(materialName) || string.IsNullOrEmpty(unit))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Проверка на количество
                if (quantity <= 0)
                {
                    MessageBox.Show("Количество должно быть больше нуля.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Проверка, что название материала не состоит только из цифр
                if (materialName.All(char.IsDigit))
                {
                    MessageBox.Show("Название материала не может состоять только из цифр.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Обновляем свойства редактируемого материала
                currentEditingMaterial.Name = materialName;
                currentEditingMaterial.Unit = unit;
                currentEditingMaterial.Quantity = quantity;

                // Обновляем данные в DataGridView
                UpdateMaterialsGrid(currentEstimate.Materials); // Обновляем таблицу материалов

                // Сбрасываем текущий редактируемый материал
                currentEditingMaterial = null;

                MessageBox.Show("Материал успешно обновлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Нет редактируемого материала.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}