Project Name: Test Automation Suite
Overview
This suite provides end-to-end testing for https://restful-booker.herokuapp.com. It utilizes NUnit as the test runner, FluentAssertions for readable validation, and a custom ResourceRegistry for automated cleanup of test entities.

Tech Stack
Framework: .NET 8 / C#

Test Runner: NUnit

Assertions: FluentAssertions

Logging: Serilog

HTTP Client: [e.g., RestSharp or HttpClient]

Getting Started
Prerequisites
.NET SDK 8.0+

[IDE (VS 2022, JetBrains Rider, or VS Code)]

Access to the Test Environment.

Installation
Clone the repository:

Bash
git clone https://github.com/rozalya/RestfulBooker.Automation
Restore dependencies:

Bash
dotnet restore

Update appsettings.json with your credentials/base URLs.

Core Components
1. Resource Cleanup
We implement a BaseCleanup logic that ensures data integrity.

How it works: All created entities are registered in ResourceRegistry.IdsToDelete.

Cleanup: The [TearDown] attribute automatically triggers BaseCleanup() to delete resources via API after each test, regardless of pass/fail status.

2. Logging and Reporting
Logs are captured during test execution via Serilog.

Test results include attachments for easier debugging. See GlobalTearDown() in BaseTest.cs to see how logs are appended to the test report context.

Running Tests

Via CLI
Run all tests:
Bash
dotnet test

Via IDE
Open the Test Explorer in Visual Studio or Rider and click "Run All."

Troubleshooting
"File in use" errors: If running in parallel, ensure your log paths include a unique identifier (e.g., test-run-{guid}.log).

Cleanup failures: Check the "Execution Logs" attachment in your test report to see the specific error returned by the API during the DeleteBookingAsync call.

Best Practices for Contributors
Always register IDs in the ResourceRegistry immediately after creation.

Keep assertions clear: Use fluentassertions with .Because(...) descriptions for better readability in CI/CD reports.

Don't break the TearDown: Ensure any new [TearDown] methods call the base implementation to avoid leaking test data in the database.
