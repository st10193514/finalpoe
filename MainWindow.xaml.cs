using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecipeApp
{
    /// Interaction logic for MainWindow.xaml

    public partial class MainWindow : Window
    {
        //Variable holds number of ingredients
        public int numIngredients = 0;
        //Varible holds number of steps
        public int numSteps = 0;
        //Variable holds users response to clearing recipe (Yes/No)
        public string? clearConfirm = null;
        //Declaring instance of Recipe class
        public Recipe recipe;
        //Variable holds the recipeIDCounter
        static int recipeIDCounter = 1;
        //Declaring delegate specifying RecipeExceededCaloriesHandler method's signature. Delegate include recipeNmae and totalCalories as parameters
        public delegate void RecipeExceededCaloriesEventHandler(string recipeName, double totalCalories);
        //Declaring event of delegate type
        public event RecipeExceededCaloriesEventHandler RecipeExceededCaloriesEvent;

        public MainWindow()
        {
            InitializeComponent();
            recipe = new Recipe(recipeIDCounter, "");
            RecipeExceededCaloriesEvent += RecipeExceededCaloriesHandler;
            ActionsMenu(GetRecipeDetails());
        }

        private Recipe GetRecipeDetails()
        {
            // Display a message box to welcome the user
            MessageBox.Show("Welcome to recipe manager!");

            // Request recipe name from the user using an input dialog
            string recipeName = Microsoft.VisualBasic.Interaction.InputBox("Enter the name of the recipe:", "Recipe Name");

            // Create a new instance of the Recipe class
            Recipe recipe = new Recipe(recipeIDCounter, recipeName);

            // Increment the recipeIDCounter
            recipeIDCounter++;

            // Request number of ingredients from the user using an input dialog
            string numIngredientsInput = Microsoft.VisualBasic.Interaction.InputBox($"Enter the number of ingredients for '{recipeName}':", "Number of Ingredients");
            this.numIngredients = int.Parse(numIngredientsInput);

            // Loop to get and store details for each ingredient
            for (int i = 0; i < this.numIngredients; i++)
            {
                // Create a new window for ingredient details
                var ingredientWindow = new Window()
                {
                    Title = $"Ingredient {i + 1} Details",
                    Width = 400,
                    Height = 400,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };

                // Create controls for ingredient details
                var nameLabel = new Label() { Content = "Name:" };
                var nameTextBox = new TextBox();

                var quantityLabel = new Label() { Content = "Quantity:" };
                var quantityTextBox = new TextBox();

                var unitLabel = new Label() { Content = "Unit of Measurement:" };
                var unitTextBox = new TextBox();

                var caloriesLabel = new Label() { Content = "Calories:" };
                var caloriesTextBox = new TextBox();

                var foodGroupLabel = new Label() { Content = "Food Group:" };
                var foodGroupComboBox = new ComboBox()
                {
                    ItemsSource = new[]
                    {
                    "Starchy foods",
                    "Vegetables and fruits",
                    "Dry beans, peas, lentils and soya",
                    "Chicken, fish, meat and eggs",
                    "Milk and dairy products",
                    "Fats and oil",
                    "Water"
                    }
                };

                var saveButton = new Button() { Content = "Save" };
                saveButton.Click += (sender, e) =>
                {
                    // Get the ingredient details from the input controls
                    string name = nameTextBox.Text;
                    double quantity = double.Parse(quantityTextBox.Text);
                    string unit = unitTextBox.Text;
                    int calories = int.Parse(caloriesTextBox.Text);
                    string foodGroup = foodGroupComboBox.SelectedItem.ToString();

                    // Create an Ingredient object and add it to the recipe
                    Ingredient ingredient = new Ingredient(name, quantity, unit, calories, foodGroup);
                    recipe.AddIngredient(recipe,ingredient);

                    // Close the ingredient window
                    ingredientWindow.Close();
                };

                // Create a stack panel to hold the controls
                var stackPanel = new StackPanel()
                {
                    Margin = new Thickness(20)
                };

                // Add the controls to the stack panel
                stackPanel.Children.Add(nameLabel);
                stackPanel.Children.Add(nameTextBox);
                stackPanel.Children.Add(quantityLabel);
                stackPanel.Children.Add(quantityTextBox);
                stackPanel.Children.Add(unitLabel);
                stackPanel.Children.Add(unitTextBox);
                stackPanel.Children.Add(caloriesLabel);
                stackPanel.Children.Add(caloriesTextBox);
                stackPanel.Children.Add(foodGroupLabel);
                stackPanel.Children.Add(foodGroupComboBox);
                stackPanel.Children.Add(saveButton);

                // Set the content of the ingredient window as the stack panel
                ingredientWindow.Content = stackPanel;

                // Show the ingredient window as a dialog
                ingredientWindow.ShowDialog();
            }

            // Request number of steps from the user using an input dialog
            string numStepsInput = Microsoft.VisualBasic.Interaction.InputBox("Enter the number of steps:", "Number of Steps");
            int numSteps = int.Parse(numStepsInput);

            // Loop to get and store descriptions for each step
            for (int i = 0; i < numSteps; i++)
            {
                string description = Microsoft.VisualBasic.Interaction.InputBox($"Enter description for step {i + 1}:", "Step Description");

                // Add the step description to the recipe
                recipe.AddStep(recipe,description);
            }

            // Call the AddRecipe method to add the recipe to the recipes list
            recipe.AddRecipe(recipe);

            // Add the recipe to the recipeDictionary
            recipe.recipeDictionary.Add(recipe.RecipeID, recipe);

            // If the total calories exceed 300, raise the RecipeExceededCaloriesEvent
            if (recipe.CalculateTotalCalories(recipe) > 300)
            {
                RecipeExceededCaloriesEvent?.Invoke(recipeName, recipe.CalculateTotalCalories(recipe));
            }

            // Display a message box to confirm the added recipe
            MessageBox.Show($"Recipe '{recipeName}' added successfully");

            // Display the recipe by calling DisplayRecipe method
            recipe.DisplayRecipe(recipe);

            // Return the recipe object
            return recipe;
        }

        private void ActionsMenu(Recipe recipe)
        {
            var run = true;
            bool clearAndEnterNewRecipe = false;
            bool addNewRecipe = false;
            // Declare the actionsWindow variable
            Window actionsWindow = null;

            while (run)
            {
                // Create the controls for the actions menu
                var titleLabel = new Label() { Content = "Please select one of the following:" };

                var scaleButton = new Button() { Content = "Scale Recipe" };
                scaleButton.Click += (sender, e) =>
                {
                    // Create the scaling window
                    var scalingWindow = new Window()
                    {
                        Title = "Scale Recipe",
                        Width = 300,
                        Height = 200,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };

                    // Create the controls for scaling
                    var scalingLabel = new Label() { Content = "Enter scaling factor (0.5, 2, or 3):" };
                    var scalingTextBox = new TextBox();

                    var scaleButton = new Button() { Content = "Scale" };
                    scaleButton.Click += (innerSender, innerE) =>
                    {
                        // Handle scaling the recipe here...
                        if (double.TryParse(scalingTextBox.Text, out double scalingFactor))
                        {
                            // Call the method to scale the recipe using the scalingFactor
                            recipe.ScaleRecipe(scalingFactor, recipe);

                            // Display the scaled recipe
                            recipe.DisplayRecipe(recipe);

                            // Close the scaling window
                            scalingWindow.Close();
                        }
                        else
                        {
                            MessageBox.Show("Invalid scaling factor. Please enter a valid number.");
                        }
                    };

                    // Add the controls to a stack panel
                    var scalingStackPanel = new StackPanel()
                    {
                        Margin = new Thickness(20)
                    };
                    scalingStackPanel.Children.Add(scalingLabel);
                    scalingStackPanel.Children.Add(scalingTextBox);
                    scalingStackPanel.Children.Add(scaleButton);

                    // Set the content of the scaling window
                    scalingWindow.Content = scalingStackPanel;

                    // Show the scaling window as a dialog
                    scalingWindow.ShowDialog();
                };

                var resetButton = new Button() { Content = "Reset Quantities" };
                resetButton.Click += (sender, e) =>
                {
                    // Create the reset window
                    var resetWindow = new Window()
                    {
                        Title = "Reset Recipe Quantities",
                        Width = 300,
                        Height = 200,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };

                    var resetButton = new Button() { Content = "Reset quantities" };
                    resetButton.Click += (innerSender, innerE) =>
                    {
                        //Calling ResetQuantities method 
                        recipe.ResetQuantities();

                        // Display the scaled recipe
                        recipe.DisplayRecipe(recipe);

                        // Close the scaling window
                        resetWindow.Close();
                    };

                    // Add the controls to a stack panel
                    var resetStackPanel = new StackPanel()
                    {
                        Margin = new Thickness(20)
                    };
                    resetStackPanel.Children.Add(resetButton);

                    // Set the content of the scaling window
                    resetWindow.Content = resetStackPanel;

                    // Show the scaling window as a dialog
                    resetWindow.ShowDialog();
                };

                var clearButton = new Button() { Content = "Clear Recipe" };
                clearButton.Click += (sender, e) =>
                {
                    // Create the confirmation window and controls
                    var confirmWindow = new Window()
                    {
                        Title = "Confirm Clear",
                        Width = 300,
                        Height = 200,
                        WindowStartupLocation = WindowStartupLocation.CenterOwner,
                        Owner = actionsWindow  // Set the owner window appropriately
                    };

                    var confirmTextBlock = new TextBlock()
                    {
                        Text = "Are you sure you want to clear the current recipe?",
                        Margin = new Thickness(20),
                        TextWrapping = TextWrapping.Wrap
                    };

                    var yesButton = new Button()
                    {
                        Content = "Yes",
                        Width = 80,
                        Margin = new Thickness(20),
                        HorizontalAlignment = HorizontalAlignment.Left
                    };
                    yesButton.Click += (confirmSender, confirmArgs) =>
                    {
                        // Set the clearAndEnterNewRecipe variable to true if confirmed
                        clearAndEnterNewRecipe = true;

                        // Call the ClearRecipe method
                        recipe.ClearRecipe(recipe);

                        // Close the actions window
                        actionsWindow.Close();

                        // Display the statement confirming recipe clearance
                        MessageBox.Show("Recipe cleared successfully");

                        // Call the GetRecipeDetails method to get ingredients and steps from the user and return the new recipe object
                        var newRecipe = GetRecipeDetails();

                        // Close the confirmation window
                        confirmWindow.Close();

                        // Call the ActionsMenu method with the new recipe object as an argument to continue the process
                        ActionsMenu(newRecipe);
                    };

                    var noButton = new Button()
                    {
                        Content = "No",
                        Width = 80,
                        Margin = new Thickness(20),
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    noButton.Click += (confirmSender, confirmArgs) =>
                    {
                        confirmWindow.Close();
                    };

                    var buttonsStackPanel = new StackPanel()
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Margin = new Thickness(0, 20, 0, 0)
                    };
                    buttonsStackPanel.Children.Add(yesButton);
                    buttonsStackPanel.Children.Add(noButton);

                    var confirmStackPanel = new StackPanel()
                    {
                        Margin = new Thickness(10)
                    };
                    confirmStackPanel.Children.Add(confirmTextBlock);
                    confirmStackPanel.Children.Add(buttonsStackPanel);

                    confirmWindow.Content = confirmStackPanel;

                    confirmWindow.ShowDialog();
                };

                var addButton = new Button() { Content = "Add Recipe" };
                addButton.Click += (sender, e) =>
                {
                    // Create the confirmation window
                    MessageBoxResult result = MessageBox.Show("Are you sure you want to add a new recipe?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    // Check the user's response
                    if (result == MessageBoxResult.Yes)
                    {
                        // Close the actions window
                        actionsWindow.Close();

                        // Call the GetRecipeDetails method to get ingredients and steps from the user and return the new recipe object
                        var newRecipe = GetRecipeDetails();

                        // Call the ActionsMenu method with the new recipe object as an argument to continue the process
                        ActionsMenu(newRecipe);
                    }
                };

                var displayAllButton = new Button() { Content = "Display all Recipes" };
                displayAllButton.Click += (sender, e) =>
                {
                    // Create a ListBox control to display the recipe list
                    var listBox = new ListBox
                    {
                        Width = 400,
                        Height = 250
                    };

                    // Call the DisplayRecipeList method to populate the listBox
                    recipe.DisplayRecipeList(listBox);

                    // Create a TextBlock for the prompt
                    var promptTextBlock = new TextBlock
                    {
                        Text = "Enter the number of the recipe to view:",
                        Margin = new Thickness(0, 10, 0, 0)
                    };

                    // Create a TextBox for the user input
                    var recipeNumberTextBox = new TextBox
                    {
                        Width = 150,
                        Margin = new Thickness(0, 5, 0, 0)
                    };

                    // Create an "Enter" button
                    var enterButton = new Button
                    {
                        Content = "Enter",
                        Width = 80,
                        Margin = new Thickness(0, 10, 0, 0)
                    };

                    // Create a StackPanel to hold the prompt, TextBox, and Enter button
                    var stackPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness(10)
                    };
                    stackPanel.Children.Add(promptTextBlock);
                    stackPanel.Children.Add(recipeNumberTextBox);
                    stackPanel.Children.Add(enterButton);

                    // Create a Grid to hold the listBox and stackPanel
                    var grid = new Grid();
                    grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
                    grid.Children.Add(listBox);
                    grid.Children.Add(stackPanel);
                    Grid.SetRow(listBox, 0);
                    Grid.SetRow(stackPanel, 1);

                    // Create a Window to display the recipe list
                    var recipeListWindow = new Window
                    {
                        Title = "All Recipes",
                        Content = grid,
                        Width = 400,
                        Height = 400
                    };

                    // Event handler for the Enter button click
                    enterButton.Click += (s, args) =>
                    {
                        // Get the recipe number input
                        string recipeNumberInput = recipeNumberTextBox.Text;

                        // Parse the recipe number input
                        if (int.TryParse(recipeNumberInput, out int recipeNumber) && recipe.recipeDictionary.ContainsKey(recipeNumber))
                        {
                            // Call the DisplaySpecificRecipe method with the chosen recipe number
                            recipe.DisplaySpecificRecipe(recipeNumber);
                        }
                        else
                        {
                            // Handle the case where the recipe number input is invalid
                            recipeDetailsTextBlock.Text = "Invalid input. Please enter a valid recipe number.";
                        }
                        recipeListWindow.Close();

                    };

                    // Show the recipeListWindow
                    recipeListWindow.ShowDialog();
                };

                var filterButton = new Button() { Content = "Filter recipes by entering maximum calories" };
                filterButton.Click += (sender, e) =>
                {
                    // Prompt the user for the maximum calories
                    string maxCaloriesInput = Microsoft.VisualBasic.Interaction.InputBox("Enter the maximum calories:", "Maximum Calories");

                    // Parse the maximum calories input
                    if (int.TryParse(maxCaloriesInput, out int maxCalories))
                    {
                        // Filter recipes by maximum calories
                        List<Recipe> filteredRecipes = recipe.FilterRecipesByMaxCalories(maxCalories);

                        // Display the filtered recipes using a MessageBox or another UI element
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine($"Recipes with maximum calories of {maxCalories}:");
                        foreach (Recipe recipe in filteredRecipes)
                        {
                            sb.AppendLine(recipe.RecipeName);
                        }
                        MessageBox.Show(sb.ToString(), "Filtered Recipes", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        // Handle the case where the maximum calories input is invalid
                        MessageBox.Show("Invalid input. Please enter a valid number for maximum calories.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                };
                // Create a new list box for recipe selection
                ListBox recipeListBox = new ListBox()
                {
                    ItemsSource = Recipe.recipes,
                    SelectionMode = SelectionMode.Multiple
                };

                var menuButton = new Button() { Content = "Select recipes for menu" };
                menuButton.Click += (sender, e) =>
                {
                    // Create a new window for recipe selection
                    Window recipeSelectionWindow = new Window()
                    {
                        Title = "Recipe Selection",
                        Width = 400,
                        Height = 300,
                        WindowStartupLocation = WindowStartupLocation.CenterScreen
                    };

                    // Create a stack panel to hold the checkboxes and confirm button
                    StackPanel stackPanel = new StackPanel();

                    // Create checkboxes for each recipe
                    foreach (Recipe recipe in Recipe.recipes)
                    {
                        CheckBox recipeCheckBox = new CheckBox()
                        {
                            Content = recipe.RecipeName
                        };

                        stackPanel.Children.Add(recipeCheckBox);
                    }

                    // Create a button to confirm the recipe selection
                    Button confirmButton = new Button
                    {
                        Content = "OK",
                        Width = 100,
                        Height = 30
                    };

                    // Handle the confirm button click event
                    confirmButton.Click += (confirmSender, confirmEventArgs) =>
                    {
                        // Get the selected recipes from the checkboxes
                        List<Recipe> selectedRecipes = new List<Recipe>();
                        foreach (var child in stackPanel.Children)
                        {
                            if (child is CheckBox checkBox && checkBox.IsChecked == true)
                            {
                                Recipe recipe = Recipe.recipes.FirstOrDefault(r => r.RecipeName == checkBox.Content.ToString());
                                if (recipe != null)
                                {
                                    selectedRecipes.Add(recipe);
                                }
                            }
                        }
/*                        // Calculate the total calories for the menu
                        double totalCalories = 0;
                        foreach (Recipe recipe in selectedRecipes)
                        {
                            totalCalories += recipe.CalculateTotalCalories(recipe);
                        }
*/

                        // Calculate the food group percentages
                        Dictionary<string, double> foodGroupPercentages = Recipe.CalculateFoodGroupPercentages(selectedRecipes);

                        // Create a new window to display the pie chart
                        Window pieChartWindow = new Window()
                        {
                            Title = "Menu Pie Chart",
                            Width = 600,
                            Height = 600,
                            WindowStartupLocation = WindowStartupLocation.CenterScreen
                        };

                        // Create a Canvas control to hold the pie chart
                        Canvas pieChartCanvas = new Canvas();

                        // Set the size of the canvas
                        double canvasSize = 300;
                        pieChartCanvas.Width = canvasSize;
                        pieChartCanvas.Height = canvasSize;

                        // Set the alignment of the pieChartCanvas to center
                        pieChartCanvas.HorizontalAlignment = HorizontalAlignment.Center;
                        pieChartCanvas.VerticalAlignment = VerticalAlignment.Center;

                        Dictionary<string, Color> foodGroupColors = new Dictionary<string, Color>()
                        {
                            { "Starchy foods", Colors.Green },
                            { "Vegetables and fruits", Colors.Red },
                            { "Dry beans, peas, lentils and soya", Colors.Yellow },
                            { "Chicken, fish, meat and eggs", Colors.Blue },
                            { "Milk and dairy products", Colors.Orange },
                            { "Fats and oil", Colors.Gray },
                            { "Water", Colors.Pink },
                        };

                        // Calculate the angles for the pie slices
                        double centerX = canvasSize / 2;
                        double centerY = canvasSize / 2;
                        double radius = Math.Min(centerX, centerY) - 10;

                        // Adjust the canvas size based on the smaller dimension
                        canvasSize = Math.Min(pieChartCanvas.Width, pieChartCanvas.Height);
                        pieChartCanvas.Width = canvasSize;
                        pieChartCanvas.Height = canvasSize;

                        double startAngle = 0;
                        foreach (var kvp in foodGroupPercentages)
                        {
                            double sweepAngle = 360 * kvp.Value / 100;

                            // Calculate the start and end points for the arc segment
                            double startX = centerX + Math.Sin(startAngle * Math.PI / 180) * radius;
                            double startY = centerY - Math.Cos(startAngle * Math.PI / 180) * radius;
                            double endX = centerX + Math.Sin((startAngle + sweepAngle) * Math.PI / 180) * radius;
                            double endY = centerY - Math.Cos((startAngle + sweepAngle) * Math.PI / 180) * radius;

                            // Retrieve the color for the food group
                            Color sliceColor = foodGroupColors[kvp.Key];

                            // Create a pie slice using a PathGeometry
                            PathGeometry sliceGeometry = new PathGeometry();
                            PathFigure sliceFigure = new PathFigure();
                            sliceFigure.StartPoint = new Point(centerX, centerY); // Center of the pie chart
                            sliceFigure.Segments.Add(new LineSegment(new Point(startX, startY), isStroked: true));
                            //sliceFigure.StartPoint = new Point(startX, startY); // Start point of the arc

                            // Create an arc segment
                            ArcSegment arcSegment = new ArcSegment(new Point(endX, endY), new Size(radius, radius), 0, sweepAngle < 180, SweepDirection.Clockwise, true);
                            sliceFigure.Segments.Add(arcSegment);
                            sliceGeometry.Figures.Add(sliceFigure);

                            // Create a Path object to display the pie slice
                            Path slicePath = new Path()
                            {
                                Fill = new SolidColorBrush(sliceColor), // Random color for each slice
                                Data = sliceGeometry
                            };

                            // Add the slice path to the pie chart canvas
                            pieChartCanvas.Children.Add(slicePath);

                            // Update the start angle for the next slice
                            startAngle += sweepAngle;
                        }

                        // Create a ListBox control for the legend
                        ListBox legendListBox = new ListBox();

                        // Set the size and appearance of the legend
                        legendListBox.Width = 200;
                        legendListBox.Margin = new Thickness(10);
                        legendListBox.BorderThickness = new Thickness(1);
                        legendListBox.BorderBrush = Brushes.Black;

                        // Add legend items to the ListBox
                        foreach (var kvp in foodGroupPercentages)
                        {
                            string foodGroup = kvp.Key;
                            double percentage = kvp.Value;
                            Color color = foodGroupColors[foodGroup];

                            // Create a StackPanel to hold the legend item
                            StackPanel legendItemPanel = new StackPanel()
                            {
                                Orientation = Orientation.Horizontal,
                                Margin = new Thickness(5)
                            };

                            // Create a Rectangle to display the color
                            Rectangle colorRectangle = new Rectangle()
                            {
                                Width = 20,
                                Height = 20,
                                Fill = new SolidColorBrush(color),
                                Margin = new Thickness(0, 0, 5, 0)
                            };

                            // Create a TextBlock to display the food group name and percentage
                            TextBlock foodGroupTextBlock = new TextBlock()
                            {
                                Text = $"{foodGroup} ({percentage}%)"
                            };

                            // Add the color rectangle and food group text to the legend item panel
                            legendItemPanel.Children.Add(colorRectangle);
                            legendItemPanel.Children.Add(foodGroupTextBlock);

                            // Add the legend item panel to the ListBox
                            legendListBox.Items.Add(legendItemPanel);
                        }

                        // Create a Grid to hold the pie chart and legend
                        Grid grid = new Grid();

                        // Create a row definition for the pie chart
                        RowDefinition pieChartRow = new RowDefinition();
                        pieChartRow.Height = new GridLength(1, GridUnitType.Star);
                        grid.RowDefinitions.Add(pieChartRow);

                        // Create a row definition for the content
                        RowDefinition contentRow = new RowDefinition();
                        contentRow.Height = new GridLength(1, GridUnitType.Auto);
                        grid.RowDefinitions.Add(contentRow);

                        // Add the pie chart canvas to the grid
                        Grid.SetRow(pieChartCanvas, 0);
                        grid.Children.Add(pieChartCanvas);

                        // Create a StackPanel to hold the legend and OK button
                        StackPanel contentStackPanel = new StackPanel()
                        { 
                            HorizontalAlignment = HorizontalAlignment.Right,
                            VerticalAlignment = VerticalAlignment.Top,
                            Margin = new Thickness(0, 0, 10, 10)
                        
                        };

                        // Update the alignment and margin to move the content to the left
                        contentStackPanel.HorizontalAlignment = HorizontalAlignment.Left;
                        contentStackPanel.Margin = new Thickness(65, 0, 10, 10); 

                        // Add the legend ListBox to the content stack panel
                        contentStackPanel.Children.Add(legendListBox);

                        // Create a button to confirm and exit the PieChartWindow
                        Button okButton = new Button()
                        {
                            Content = "OK",
                            Width = 100,
                            Height = 30,
                            Margin = new Thickness(0, 10, 0, 0)
                        };

                        // Handle the OK button click event
                        okButton.Click += (okSender, okEventArgs) =>
                        {
                            // Close the PieChartWindow
                            pieChartWindow.Close();
                        };

                        // Add the OK button to the content stack panel
                        contentStackPanel.Children.Add(okButton);

                        // Add the content stack panel to the grid
                        Grid.SetRow(contentStackPanel, 1);
                        grid.Children.Add(contentStackPanel);

                        // Create a Canvas to hold the grid
                        Canvas canvas = new Canvas();

                        // Set the desired margins for the pie chart
                        Thickness pieChartMargin = new Thickness(20, 20, 0, 0); // Adjust the values as per your requirement

                        // Set the margin for the pie chart canvas
                        pieChartCanvas.Margin = pieChartMargin;

                        // Add the grid to the canvas
                        canvas.Children.Add(grid);

                        // Set the canvas as the content of the pie chart window
                        pieChartWindow.Content = canvas;

                        // Show the pie chart window
                        pieChartWindow.ShowDialog();

                        // Close the recipe selection window
                        recipeSelectionWindow.Close();
                    };

                    // Adding confirmButton to stackPanel
                    stackPanel.Children.Add(confirmButton);

                    recipeSelectionWindow.Content = stackPanel;

                    // Show the recipe selection window
                    recipeSelectionWindow.ShowDialog();
                };

                var exitButton = new Button() { Content = "Exit" };
                exitButton.Click += (sender, e) =>
                {
                    // Set the "run" variable to false to exit the application
                    run = false;
                    // Close the actions window
                    actionsWindow.Close();

                    // Terminate the application
                    Application.Current.Shutdown();
                };

                // Add the buttons to a stack panel
                var actionsStackPanel = new StackPanel()
                {
                    Margin = new Thickness(20)
                };
                actionsStackPanel.Children.Add(titleLabel);
                actionsStackPanel.Children.Add(scaleButton);
                actionsStackPanel.Children.Add(resetButton);
                actionsStackPanel.Children.Add(clearButton);
                actionsStackPanel.Children.Add(addButton);
                actionsStackPanel.Children.Add(displayAllButton);
                actionsStackPanel.Children.Add(filterButton);
                actionsStackPanel.Children.Add(menuButton);
                actionsStackPanel.Children.Add(exitButton);

                // Create the actions window
                actionsWindow = new Window()
                {
                    Title = "Actions Menu",
                    Width = 400,
                    Height = 300,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Content = actionsStackPanel
                };

                // Show the actions window as a dialog
                actionsWindow.ShowDialog();

                //If statement to break from loop if user decides to clear and enter new recipe
                if (clearAndEnterNewRecipe)
                {
                    break;
                }

                //If statement to break from loop if user decides to add a new recipe
                if (addNewRecipe)
                {
                    break;
                }
            }
        }

        //Method to handle exceeded calories
        private void RecipeExceededCaloriesHandler(string recipeName, double totalCalories)
        {
            MessageBox.Show($"The total calories of '{recipeName}' exceed 300 with a total of {totalCalories} calories.", "Calorie Exceeded");
        }

        //Method to return food group colour
        private Color GetFoodGroupColor(string foodGroup)
        {
            switch (foodGroup)
            {
                case "Starchy foods":
                    return Colors.Green;
                case "Vegetables and fruits":
                    return Colors.Red;
                case "Dry beans, peas, lentils and soya":
                    return Colors.Yellow;
                case "Chicken, fish, meat and eggs":
                    return Colors.Blue;
                case "Milk and dairy products":
                    return Colors.Orange;
                case "Fats and oil":
                    return Colors.Gray;
                case "Water":
                    return Colors.Pink;
                default:
                    return Colors.Gray;
            }
        }
    }
}