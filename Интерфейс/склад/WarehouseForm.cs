using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json;
using КурсоваяПрог.Интерфейс.склад.запасы_стройматериалов;
using КурсоваяПрог.Интерфейс.склад.Поставки;
using КурсоваяПрог.Интерфейс.склад.поставщики;

namespace КурсоваяПрог.Интерфейс.склад
{
    public partial class WarehouseForm : Form
    {
        private List<ConstructionMaterial> materials; // Измените на List<ConstructionMaterial>
        private List<Supplier> suppliers = new List<Supplier>(); // Инициализация пустого списка
        private List<Delivery> deliveries;
        private DataManager dataManager;
        private string dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Data"); // Укажите путь к директории для сохранения данных
        public event Action MaterialsUpdated; // Событие для обновления материалов
        private bool isSaved; // Поле для отслеживания состояния сохранения


        public WarehouseForm()
        {
            InitializeComponent();
            dataManager = new DataManager();
            suppliers = new List<Supplier>(); // Инициализация списка поставщиков
            InitializeData();
            InitializeDataGridViews();
            LoadData(); // Загрузка данных при инициализации формы


            isSaved = false; // Изначально данные не сохранены

        }

        private void InitializeData()
        {
            materials = new List<ConstructionMaterial>(); // Инициализируем как List<ConstructionMaterial>
            suppliers = new List<Supplier>();
            deliveries = new List<Delivery>();
        }

        private void InitializeDataGridViews()
        {
            // Настройка DataGridView для материалов
            dataGridViewMaterials.AutoGenerateColumns = false;
            dataGridViewMaterials.Columns.Clear();
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Название", DataPropertyName = "Name" });
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Unit", HeaderText = "Единица измерения", DataPropertyName = "Unit" });
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Количество", DataPropertyName = "Quantity" });
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "Закупочная цена", DataPropertyName = "Price" });
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Stock", HeaderText = "Остаток на складе", DataPropertyName = "Stock" });
            dataGridViewMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Supplier", HeaderText = "Поставщик", DataPropertyName = "SupplierName" }); // Изменено на SupplierName
            // Настройка DataGridView для поставщиков
            dataGridViewSuppliers.AutoGenerateColumns = false;
            dataGridViewSuppliers.Columns.Clear();
            dataGridViewSuppliers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Название", DataPropertyName = "Name" });
            dataGridViewSuppliers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Address", HeaderText = "Адрес", DataPropertyName = "Address" });
            dataGridViewSuppliers.Columns.Add(new DataGridViewTextBoxColumn { Name = "DirectorName", HeaderText = "ФИО руководителя", DataPropertyName = "DirectorName" });
            dataGridViewSuppliers.Columns.Add(new DataGridViewTextBoxColumn { Name = "DirectorPhone", HeaderText = "Телефон", DataPropertyName = "DirectorPhone" });
            dataGridViewSuppliers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Bank", HeaderText = "Банк", DataPropertyName = "Bank" });
            dataGridViewSuppliers.Columns.Add(new DataGridViewTextBoxColumn { Name = "Account", HeaderText = "Расчетный счет", DataPropertyName = "Account" });
            dataGridViewSuppliers.Columns.Add(new DataGridViewTextBoxColumn { Name = "INN", HeaderText = "ИНН", DataPropertyName = "INN" });

            // Настройка DataGridView для поставок
            // Настройка DataGridView для поставок
            dataGridViewDeliveries.AutoGenerateColumns = false;
            dataGridViewDeliveries.Columns.Clear();
            dataGridViewDeliveries.Columns.Add(new DataGridViewTextBoxColumn { Name = "Supplier", HeaderText = "Поставщик", DataPropertyName = "SupplierName" });
            dataGridViewDeliveries.Columns.Add(new DataGridViewTextBoxColumn { Name = "DeliveryDate", HeaderText = "Дата поставки", DataPropertyName = "DeliveryDate" });

            // Настройка DataGridView для полученных стройматериалов
            dataGridViewDeliveriesMaterials.AutoGenerateColumns = false;
            dataGridViewDeliveriesMaterials.Columns.Clear();
            dataGridViewDeliveriesMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Name", HeaderText = "Название", DataPropertyName = "Name" });
            dataGridViewDeliveriesMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Unit", HeaderText = "Единица измерения", DataPropertyName = "Unit" });
            dataGridViewDeliveriesMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Quantity", HeaderText = "Количество", DataPropertyName = "Quantity" });
            dataGridViewDeliveriesMaterials.Columns.Add(new DataGridViewTextBoxColumn { Name = "Price", HeaderText = "Закупочная цена", DataPropertyName = "Price" });

            // Подключение обработчика события
            dataGridViewDeliveries.SelectionChanged += dataGridViewDeliveries_SelectionChanged;
        }


        private void UpdateMaterialDataGridView() // Метод для обновления DataGridView материалов
        {
            dataGridViewMaterials.DataSource = null; // Сбрасываем источник данных
            dataGridViewMaterials.DataSource = materials; // Устанавливаем новый источник данных
        }

        private void UpdateSupplierDataGridView() // Метод для обновления DataGridView поставщиков
        {
            dataGridViewSuppliers.DataSource = null; // Сбрасываем источник данных
            dataGridViewSuppliers.DataSource = suppliers; // Устанавливаем новый источник данных
        }

        private void UpdateDeliveryDataGridView() // Метод для обновления DataGridView поставок
        {
            dataGridViewDeliveries.DataSource = null; // Сбрасываем источник данных
            if (deliveries != null && deliveries.Count > 0) // Проверяем, есть ли поставки
            {
                dataGridViewDeliveries.DataSource = deliveries; // Устанавливаем новый источник данных
            }
        }

        private void LoadData() // Метод для загрузки данных
        {
            try
            {
                suppliers = dataManager.LoadSuppliers(dataDirectory); // Загружаем поставщиков
                deliveries = dataManager.LoadDeliveries(dataDirectory); // Загружаем поставки
                materials = dataManager.LoadMaterials(dataDirectory); // Загружаем материалы

                // Проверка на null
                if (suppliers == null)
                {
                    suppliers = new List<Supplier>(); // Инициализация пустого списка, если загрузка не удалась
                }
                if (deliveries == null)
                {
                    deliveries = new List<Delivery>(); // Инициализация пустого списка, если загрузка не удалась
                }
                if (materials == null)
                {
                    materials = new List<ConstructionMaterial>(); // Инициализация пустого списка, если загрузка не удалась
                }

                UpdateSupplierDataGridView(); // Обновляем отображение поставщиков
                UpdateDeliveryDataGridView(); // Обновляем отображение поставок
                UpdateMaterialDataGridView(); // Обновляем отображение материалов
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Обработка ошибок
            }
        }

        // Кнопки для работы с запасами стройматериалов
        private void btnAddMaterial_Click(object sender, EventArgs e)
        {
            using (var form = new AddMaterialForm(suppliers)) // Открываем форму добавления материала
            {
                if (form.ShowDialog() == DialogResult.OK) // Если форма закрыта с результатом OK
                {
                    materials.Add(form.NewMaterial); // Добавляем новый материал в список
                    UpdateMaterialDataGridView(); // Обновляем отображение в DataGridView
                    MaterialsUpdated?.Invoke(); // Вызываем событие обновления
                    dataManager.SaveToFile(Path.Combine(dataDirectory, "materials.json"), materials); // Сохраняем данные материалов
                }
            }
        }

        private void btnEditMaterial_Click(object sender, EventArgs e)
        {
            if (dataGridViewMaterials.SelectedRows.Count > 0) // Проверяем, выбрана ли строка
            {
                var selectedMaterial = (ConstructionMaterial)dataGridViewMaterials.SelectedRows[0].DataBoundItem; // Получаем выбранный материал

                using (var form = new AddMaterialForm(suppliers, selectedMaterial)) // Открываем форму редактирования материала
                {
                    if (form.ShowDialog() == DialogResult.OK) // Если форма закрыта с результатом OK
                    {
                        var index = materials.IndexOf(selectedMaterial); // Находим индекс выбранного материала
                        materials[index] = form.NewMaterial; // Обновляем материал в списке
                        UpdateMaterialDataGridView(); // Обновляем отображение в DataGridView
                        dataManager.SaveToFile(Path.Combine(dataDirectory, "materials.json"), materials); // Сохраняем данные материалов
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите материал для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Сообщение об ошибке
            }
        }

        private void btnDeleteMaterial_Click(object sender, EventArgs e)
        {
            if (dataGridViewMaterials.SelectedRows.Count > 0) // Проверяем, выбрана ли строка
            {
                var selectedMaterial = (ConstructionMaterial)dataGridViewMaterials.SelectedRows[0].DataBoundItem; // Получаем выбранный материал
                materials.Remove(selectedMaterial); // Удаляем материал из списка
                UpdateMaterialDataGridView(); // Обновляем отображение в DataGridView
                dataManager.SaveToFile(Path.Combine(dataDirectory, "materials.json"), materials); // Сохраняем данные материалов
            }
        }

        // Кнопки для работы с поставками


        private void btnAddDelivery_Click(object sender, EventArgs e)
        {
            using (var form = new AddDeliveryForm(suppliers)) // Открываем форму добавления поставки
            {
                if (form.ShowDialog() == DialogResult.OK) // Если форма закрыта с результатом OK
                {
                    deliveries.Add(form.NewDelivery); // Добавляем новую поставку в список
                    UpdateDeliveryDataGridView(); // Обновляем отображение в DataGridView
                    dataManager.SaveDeliveries(dataDirectory, deliveries); // Сохраняем данные поставок
                }
            }
        }

        private void btnEditDelivery_Click(object sender, EventArgs e)
        {
            if (dataGridViewDeliveries.SelectedRows.Count > 0) // Проверяем, выбрана ли строка
            {
                var selectedDelivery = (Delivery)dataGridViewDeliveries.SelectedRows[0].DataBoundItem; // Получаем выбранную поставку
                using (var form = new AddDeliveryForm(suppliers, selectedDelivery)) // Открываем форму редактирования поставки
                {
                    if (form.ShowDialog() == DialogResult.OK) // Если форма закрыта с результатом OK
                    {
                        var index = deliveries.IndexOf(selectedDelivery); // Находим индекс выбранной поставки
                        deliveries[index] = form.NewDelivery; // Обновляем поставку в списке
                        UpdateDeliveryDataGridView(); // Обновляем отображение в DataGridView
                        dataManager.SaveDeliveries(dataDirectory, deliveries); // Сохраняем данные поставок
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите поставку для редактирования.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning); // Сообщение об ошибке
            }
        }


        private void btnDeleteDelivery_Click(object sender, EventArgs e)
        {
            if (dataGridViewDeliveries.SelectedRows.Count > 0) // Проверяем, выбрана ли строка
            {
                var selectedDelivery = (Delivery)dataGridViewDeliveries.SelectedRows[0].DataBoundItem; // Получаем выбранную поставку
                deliveries.Remove(selectedDelivery); // Удаляем поставку из списка
                UpdateDeliveryDataGridView(); // Обновляем отображение в DataGridView
                dataManager.SaveDeliveries(dataDirectory, deliveries); // Сохраняем данные поставок
            }
        }


        private void dataGridViewDeliveries_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewDeliveries.SelectedRows.Count > 0) // Проверяем, выбрана ли строка
            {
                var selectedDelivery = (Delivery)dataGridViewDeliveries.SelectedRows[0].DataBoundItem; // Получаем выбранную поставку
                if (selectedDelivery != null) // Если выбранная поставка не null
                {
                    UpdateMaterialsForSelectedDelivery(selectedDelivery); // Обновляем материалы для выбранной поставки
                }
            }
            else
            {
                dataGridViewDeliveriesMaterials.DataSource = null; // Очистить, если нет выбранной строки
            }
        }

        private void UpdateMaterialsForSelectedDelivery(Delivery selectedDelivery)
        {
            if (selectedDelivery != null && selectedDelivery.ReceivedMaterials != null) // Проверяем, есть ли материалы
            {
                dataGridViewDeliveriesMaterials.DataSource = null; // Очистить существующие данные
                dataGridViewDeliveriesMaterials.DataSource = selectedDelivery.ReceivedMaterials; // Установить новый источник данных
            }
            else
            {
                dataGridViewDeliveriesMaterials.DataSource = null; // Очистить, если нет материалов
            }
        }


        // Кнопки для работы с поставщиками
        private void btnAddSupplier_Click(object sender, EventArgs e)
        {
            using (var form = new AddSupplierForm()) // Открываем форму добавления поставщика
            {
                if (form.ShowDialog() == DialogResult.OK) // Если форма закрыта с результатом OK
                {
                    // Проверяем, что NewSupplier не равен null
                    if (form.NewSupplier != null) // Если новый поставщик не null
                    {
                        suppliers.Add(form.NewSupplier); // Добавляем нового поставщика в список
                        UpdateSupplierDataGridView(); // Обновляем отображение в DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Ошибка: новый поставщик не был создан.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Сообщение об ошибке
                    }
                }
            }
        }

        private void btnEditSupplier_Click(object sender, EventArgs e)
        {
            if (dataGridViewSuppliers.SelectedRows.Count > 0) // Проверяем, выбрана ли строка
            {
                var selectedSupplier = (Supplier)dataGridViewSuppliers.SelectedRows[0].DataBoundItem; // Получаем выбранного поставщика
                using (var form = new AddSupplierForm(selectedSupplier)) // Открываем форму редактирования поставщика
                {
                    if (form.ShowDialog() == DialogResult.OK) // Если форма закрыта с результатом OK
                    {
                        var index = suppliers.IndexOf(selectedSupplier); // Находим индекс выбранного поставщика
                        suppliers[index] = form.NewSupplier; // Обновляем поставщика в списке
                        UpdateSupplierDataGridView(); // Обновляем отображение в DataGridView
                    }
                }
            }
        }

        private void btnDeleteSupplier_Click(object sender, EventArgs e)
        {
            if (dataGridViewSuppliers.SelectedRows.Count > 0) // Проверяем, выбрана ли строка
            {
                var selectedSupplier = (Supplier)dataGridViewSuppliers.SelectedRows[0].DataBoundItem; // Получаем выбранного поставщика
                suppliers.Remove(selectedSupplier); // Удаляем поставщика из списка
                UpdateSupplierDataGridView(); // Обновляем отображение в DataGridView
            }
        }
        // Кнопки для сохранения и загрузки данных
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                dataManager.SaveSuppliers(dataDirectory, suppliers); // Сохраняем данные поставщиков
                SaveDeliveriesToJson(); // Сохраняем данные поставок в deliveries.json
                dataManager.SaveToFile(Path.Combine(dataDirectory, "materials.json"), materials); // Сохраняем данные материалов
                MessageBox.Show("Данные успешно сохранены.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information); // Сообщение об успешном сохранении
                isSaved = true; // Устанавливаем, что данные сохранены
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); // Обработка ошибок
            }
        }

        private void SaveDeliveriesToJson()
        {
            string deliveriesJson = JsonConvert.SerializeObject(deliveries, Formatting.Indented);
            File.WriteAllText(Path.Combine(dataDirectory, "deliveries.json"), deliveriesJson);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Если данные не сохранены, выполняем автоматическое сохранение
            if (!isSaved)
            {
                var result = MessageBox.Show("Данные не сохранены. Вы хотите сохранить изменения?", "Подтверждение", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    btnSave.PerformClick(); // Вызываем метод сохранения
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true; // Отменяем закрытие формы
                }
            }
        }

        private void dataGridViewDeliveries_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void WarehouseForm_Load(object sender, EventArgs e)
        {

        }
    }
}