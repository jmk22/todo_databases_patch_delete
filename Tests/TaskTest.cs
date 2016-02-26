using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Xunit;

namespace ToDoList
{
  public class ToDoTest : IDisposable
  {
    public ToDoTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=todo_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      //Arrange, Act
      int result = Task.GetAll().Count;

      //Assert
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfDescriptionsAreTheSame()
    {
      //Arrange, Act
      Task firstTask = new Task("Mow the lawn", 1);
      Task secondTask = new Task("Mow the lawn", 1);

      //Assert
      Assert.Equal(firstTask, secondTask);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", 1);

      //Act
      testTask.Save();
      List<Task> result = Task.GetAll();
      List<Task> testList = new List<Task>{testTask};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", 1);

      //Act
      testTask.Save();
      Task savedTask = Task.GetAll()[0];

      int result = savedTask.GetId();
      int testId = testTask.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsTaskInDatabase()
    {
      //Arrange
      Task testTask = new Task("Mow the lawn", 1);
      testTask.Save();

      //Act
      Task foundTask = Task.Find(testTask.GetId());

      //Assert
      Assert.Equal(testTask, foundTask);
    }

    [Fact]
    public void Test_Update_UpdatesTaskInDatabase()
    {
      //Arrange
      Task testTask = new Task("Walk the dog", 1);
      testTask.Save();

      //Act
      string newDescription = "Walk the giraffe";
      testTask.Update(newDescription);

      string result = testTask.GetDescription();

      //Assert
      Assert.Equal(newDescription, result);
    }

    [Fact]
    public void Test_Delete_DeletesTaskFromDatabase()
    {
      //Arrange
      Task testTask1 = new Task("Walk the dog", 1);
      testTask1.Save();

      Task testTask2 = new Task("Mow the lawn", 1);
      testTask2.Save();

      //Act
      testTask1.Delete();
      List<Task> resultTasks = Task.GetAll();
      List<Task> testTaskList = new List<Task> {testTask2};

      //Assert
      Assert.Equal(testTaskList, resultTasks);
    }

    public void Dispose()
    {
      Task.DeleteAll();
    }
  }
}
