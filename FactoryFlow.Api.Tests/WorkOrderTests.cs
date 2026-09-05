using FactoryFlow.Api.Models;

namespace FactoryFlow.Api.Tests;

public class WorkOrderTests
{
    [Fact]
    public void Start_WhenStatusIsOpen_ChangesStatusToInProgress()
    {
        WorkOrder workOrder = new(1, "Repair motor", "Motor noise is too loud.");

        workOrder.Start();

        Assert.Equal(WorkOrderStatus.InProgress, workOrder.Status);
        Assert.Null(workOrder.CompletedAt);
    }

    [Fact]
    public void Complete_WhenStatusIsInProgress_ChangesStatusToCompletedAndSetsCompletedAt()
    {
        WorkOrder workOrder = new(1, "Repair motor", "Motor noise is too loud.");
        workOrder.Start();

        workOrder.Complete();

        Assert.Equal(WorkOrderStatus.Completed, workOrder.Status);
        Assert.NotNull(workOrder.CompletedAt);
    }

    [Fact]
    public void Cancel_WhenStatusIsCompleted_ThrowsInvalidOperationException()
    {
        WorkOrder workOrder = new(1, "Repair motor", "Motor noise is too loud.");
        workOrder.Start();
        workOrder.Complete();

        Assert.Throws<InvalidOperationException>(() => workOrder.Cancel());
    }
}
