using System;
using System.Collections.Generic;

namespace RecipeApp
{
    public class Recipe
    {
        public string Name { get; set; }
        public List<string> Ingredients { get; set; }
        public string FoodGroup { get; set; }
        public int Calories { get; set; }
    }

    public class RecipeFilter
    {
        public List<Recipe> FilterByIngredient(List<Recipe> recipes, string ingredient)
        {
            List<Recipe> filteredRecipes = new List<Recipe>();

            foreach (Recipe recipe in recipes)
            {
                if (recipe.Ingredients.Contains(ingredient))
                {
                    filteredRecipes.Add(recipe);
                }
            }

            return filteredRecipes;
        }

        public List<Recipe> FilterByFoodGroup(List<Recipe> recipes, string foodGroup)
        {
            List<Recipe> filteredRecipes = new List<Recipe>();

            foreach (Recipe recipe in recipes)
            {
                if (recipe.FoodGroup.Equals(foodGroup, StringComparison.OrdinalIgnoreCase))
                {
                    filteredRecipes.Add(recipe);
                }
            }

            return filteredRecipes;
        }

        public List<Recipe> FilterByMaxCalories(List<Recipe> recipes, int maxCalories)
        {
            List<Recipe> filteredRecipes = new List<Recipe>();

            foreach (Recipe recipe in recipes)
            {
                if (recipe.Calories <= maxCalories)
                {
                    filteredRecipes.Add(recipe);
                }
            }

            return filteredRecipes;
        }
    }

    public class RecipeApp
    {
        private List<Recipe> recipes;

        public RecipeApp()
        {
            recipes = new List<Recipe>();
        }

        public void AddRecipe(string name, List<string> ingredients, string foodGroup, int calories)
        {
            Recipe recipe = new Recipe
            {
                Name = name,
                Ingredients = ingredients,
                FoodGroup = foodGroup,
                Calories = calories
            };

            recipes.Add(recipe);
        }

        public List<Recipe> GetRecipes()
        {
            return recipes;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            RecipeApp recipeApp = new RecipeApp();
            RecipeFilter recipeFilter = new RecipeFilter();

            // Add sample recipes
            recipeApp.AddRecipe("Pasta Carbonara", new List<string> { "pasta", "bacon", "eggs", "cheese" }, "Main Dish", 500);
            recipeApp.AddRecipe("Caesar Salad", new List<string> { "lettuce", "croutons", "parmesan", "chicken" }, "Salad", 300);
            recipeApp.AddRecipe("Chocolate Cake", new List<string> { "flour", "sugar", "cocoa powder", "eggs" }, "Dessert", 600);

            // Get all recipes
            List<Recipe> allRecipes = recipeApp.GetRecipes();
            Console.WriteLine("All Recipes:");
            PrintRecipes(allRecipes);

            // Filter recipes by ingredient
            List<Recipe> filteredByIngredient = recipeFilter.FilterByIngredient(allRecipes, "eggs");
            Console.WriteLine("\nRecipes filtered by ingredient 'eggs':");
            PrintRecipes(filteredByIngredient);

            // Filter recipes by food group
            List<Recipe> filteredByFoodGroup = recipeFilter.FilterByFoodGroup(allRecipes, "Salad");
            Console.WriteLine("\nRecipes filtered by food group 'Salad':");
            PrintRecipes(filteredByFoodGroup);

            // Filter recipes by max calories
            List<Recipe> filteredByMaxCalories = recipeFilter.FilterByMaxCalories(allRecipes, 400);
            Console.WriteLine("\nRecipes filtered by max calories (<= 400):");
            PrintRecipes(filteredByMaxCalories);
        }

        public static void PrintRecipes(List<Recipe> recipes)
        {
            foreach (Recipe recipe in recipes)
            {
                Console.WriteLine($"Name: {recipe.Name}");
                Console.WriteLine($"Ingredients: {string.Join(", ", recipe.Ingredients)}");
                Console.WriteLine($"Food Group: {recipe.FoodGroup}");
                Console.WriteLine($"Calories: {recipe.Calories}");
                Console.WriteLine();
            }
        }
    }
}
