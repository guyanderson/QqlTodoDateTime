using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  [Collection("ToDoTests")]
  public class CategoryTest : IDisposable
  {
    public CategoryTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_CategoriesEmptyAtFirst()
    {
      //Arrange, Act
      int result = Category.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Equal_ReturnsTrueForSameName_True()
    {
      //Arrange, Act
      Category firstCategory = new Category("Household chores");
      Category secondCategory = new Category("Household chores");
Console.WriteLine(firstCategory.GetName());
Console.WriteLine(secondCategory.GetName());
      //Assert
      Assert.Equal(firstCategory, secondCategory);
    }

    [Fact]
    public void Save_SavesToDatabase_True()
    {
      //Arrange, Act
      Category testCategory = new Category("Garden");
      testCategory.Save();

      //Act
      List<Category> result = Category.GetAll();
      List<Category> testList = new List<Category>{testCategory};
Console.WriteLine(result[0].GetName());
Console.WriteLine(testList[0].GetName());
      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Find_FindsCategoryInDatabase_True()
    {
      Category testCategory = new Category("Laundry");
      testCategory.Save();

      Category foundCategory = Category.Find(testCategory.GetId());
Console.WriteLine(testCategory.GetName());
Console.WriteLine(foundCategory.GetName());

      Assert.Equal(testCategory, foundCategory);
    }

    [Fact]
    public void Update_UpdatesCategoryInDatabase_True()
    {
      string name = "Home stuff";
      Category testCategory = new Category(name);
      testCategory.Save();
      string newName = "Work stuff";

      testCategory.Update(newName);

      string result = testCategory.GetName();
//Console.WriteLine(result.GetName());
//Console.WriteLine(newName.GetName());
      Assert.Equal(newName, result);
    }

    [Fact]
    public void AddTask_AddsTaskToCategory_True()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Task testTask = new Task("Mow the lawn", new DateTime (2017, 1, 1));
      testTask.Save();

      Task testTask2 = new Task("Water the garden", new DateTime (2017, 1, 1));
      testTask2.Save();

      //Act
      testCategory.AddTask(testTask);
      testCategory.AddTask(testTask2);

      List<Task> result = testCategory.GetTasks();
      List<Task> testList = new List<Task>{testTask, testTask2};
Console.WriteLine(result[0].GetDescription());
Console.WriteLine(testList[0].GetDescription());

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void AddTask_ReturnsAllCategoryTasks_True()
    {
      //Arrange
      Category testCategory = new Category("Household chores");
      testCategory.Save();

      Task testTask1 = new Task("Mow the lawn", new DateTime (2017, 1, 1));
      testTask1.Save();

      Task testTask2 = new Task("Buy plane ticket", new DateTime (2017, 1, 1));
      testTask2.Save();

      //Act
      testCategory.AddTask(testTask1);
      List<Task> savedTasks = testCategory.GetTasks();
      List<Task> testList = new List<Task> {testTask1};
Console.WriteLine(savedTasks[0].GetDescription());
Console.WriteLine(testList[0].GetDescription());
      //Assert
      Assert.Equal(testList, savedTasks);
    }

    [Fact]
    public void Delete_DeletesCategoryFromDatabase_True()
    {
      string name1 = "Home stuff";
      Category testCategory1 = new Category(name1);
      testCategory1.Save();

      string name2 = "Work stuff";
      Category testCategory2 = new Category(name2);
      testCategory2.Save();

      testCategory1.Delete();
      List<Category> resultCategories = Category.GetAll();
      List<Category> testCategoryList = new List<Category> {testCategory2};

      Assert.Equal(testCategoryList, resultCategories);
    }

    [Fact]
    public void Delete_DeletesCategoryAssociationsFromDatabase_True()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", new DateTime (2017, 1, 1));
      testTask.Save();

      string testName = "Home stuff";
      Category testCategory = new Category(testName);
      testCategory.Save();

      //Act
      testCategory.AddTask(testTask);
      testCategory.Delete();

      List<Category> resultTaskCategories = testTask.GetCategories();
      List<Category> testTaskCategories = new List<Category> {};

      //Assert
      Assert.Equal(testTaskCategories, resultTaskCategories);
    }

    public void Dispose()
    {
  //    Console.WriteLine("categories");
      Category.DeleteAll();
      Task.DeleteAll();
//      Console.WriteLine(Task.GetAll().Count);
//      Console.WriteLine(Category.GetAll().Count);

    }
  }
}
