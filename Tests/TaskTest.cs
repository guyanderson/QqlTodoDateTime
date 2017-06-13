using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ToDoList
{
  [Collection("ToDoTests")]
  public class TaskTest : IDisposable
  {
    public TaskTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void GetAll_DatabaseEmptyAtFirst_0()
    {
      //Arrange, Act
      int result = Task.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Equals_TrueForSameDescription_Task()
    {
      //Arrange, Act
      Task firstTask = new Task("Mow the lawn", new DateTime (2017, 1, 1), false);
      Task secondTask = new Task("Mow the lawn", new DateTime (2017, 1, 1), false);

      //Assert
      Assert.Equal(firstTask, secondTask);
    }

    [Fact]
    public void Save_TaskSavesToDatabase_TaskList()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", new DateTime (2017, 1, 1), false);
      testTask.Save();

      //Act
      List<Task> result = Task.GetAll();
      List<Task> testList = new List<Task>{testTask};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Save_AssignsIdToObject_id()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", new DateTime (2017, 1, 1), false);
      testTask.Save();

      //Act
      Task savedTask = Task.GetAll()[0];

      int result = savedTask.GetId();
      int testId = testTask.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Find_FindsTaskInDatabase_Task()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", new DateTime (2017, 1, 1), false);
      testTask.Save();

      //Act
      Task result = Task.Find(testTask.GetId());

      //Assert
      Assert.Equal(testTask, result);
    }

    [Fact]
    public void AddCategory_AddsCategoryToTask_CategoryList()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", new DateTime (2017, 1, 1), false);
      testTask.Save();

      Category testCategory = new Category("Home stuff");
      testCategory.Save();

      //Act
      testTask.AddCategory(testCategory);

      List<Category> result = testTask.GetCategories();
      List<Category> testList = new List<Category>{testCategory};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void GetCategories_ReturnsAllTaskCategories_CategoryList()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", new DateTime (2017, 1, 1), false);
      testTask.Save();

      Category testCategory1 = new Category("Home stuff");
      testCategory1.Save();

      Category testCategory2 = new Category("Work stuff");
      testCategory2.Save();

      //Act
      testTask.AddCategory(testCategory1);
      List<Category> result = testTask.GetCategories();
      List<Category> testList = new List<Category> {testCategory1};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Delete_DeletesTaskAssociationsFromDatabase_TaskList()
    {
      //Arrange
      Category testCategory = new Category("Home stuff");
      testCategory.Save();

      string testDescription = "Mow the lawn";
      DateTime testDueDate = new DateTime (2017, 1, 1);
      bool testStatus = false;
      Task testTask = new Task(testDescription, testDueDate, testStatus);
      testTask.Save();

      //Act
      testTask.AddCategory(testCategory);
      testTask.Delete();

      List<Task> resultCategoryTasks = testCategory.GetTasks();
      List<Task> testCategoryTasks = new List<Task> {};

      //Assert
      Assert.Equal(testCategoryTasks, resultCategoryTasks);
    }

    public void Dispose()
    {
      Task.DeleteAll();
      Category.DeleteAll();
    }
  }
}
