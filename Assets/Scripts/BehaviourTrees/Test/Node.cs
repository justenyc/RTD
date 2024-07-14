using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Node
{
    public enum Status { Success, Failure, Running}

    public readonly string name;
    public readonly int priority;

    public readonly List<Node> children = new();
    protected int currentChild;

    public Node(string _name = "Node", int _priority = 0)
    {
        name = _name;
        priority = _priority;
    }

    public void AddChild(Node newChild)
    {
        children.Add(newChild);
    }

    public virtual Status Process()
    {
        return children[currentChild].Process();
    }

    public virtual void Reset()
    {
        currentChild = 0;
        foreach (var child in children)
        {
            child.Reset();
        }
    }

    public virtual Status ForceComplete()
    {
        return Status.Success;
    }
}

public class Leaf : Node
{
    readonly IStrategy strategy;

    public Leaf(string _name, IStrategy _strategy, int _priority = 0) : base(_name, _priority)
    {
        this.strategy = _strategy;
    }

    public override Status Process()
    {
        return strategy.Process();
    }

    public override void Reset()
    {
        strategy.Reset();
    }
}

public class BehaviourTree : Node
{
    public BehaviourTree(string _name) : base(_name) { }

    public override Status Process()
    {
        while(currentChild < children.Count)
        {
            var status = children[currentChild].Process();
            if(status != Status.Success)
            {
                return status;
            }
            currentChild++;
        }
        return Status.Success;
    }
}

public class Sequence : Node
{
    public Sequence(string _name, int _priority = 0) : base(_name, _priority) { }

    public override Status Process()
    {
        if(currentChild < children.Count)
        {
            switch(children[currentChild].Process())
            {
                case Status.Running:
                    return Status.Running;
                case Status.Failure:
                    Reset();
                    return Status.Failure;
                default:
                    currentChild++;
                    return currentChild == children.Count ? Status.Success : Status.Running;
            }
        }

        Reset();
        return Status.Success;
    }
}

public class Selector : Node
{
    public Selector(string name) : base(name) { }

    public override Node.Status Process()
    {
        if (currentChild < children.Count)
        {
            switch (children[currentChild].Process())
            {
                case Status.Running:
                    return Status.Running;

                case Status.Success:
                    Reset();
                    return Status.Success;

                default:
                    currentChild++;
                    return Status.Running;
            }
        }

        Reset();
        return Status.Failure;
    }
}

public class PrioritySelector  : Node
{
    List<Node> sortedChildren;
    List<Node> SortedChildren => sortedChildren ??= SortChildren();

    protected virtual List<Node> SortChildren() => children.OrderByDescending(child => child.priority).ToList();

    public override Status Process()
    {
        foreach(var child in SortedChildren) {
            switch(child.Process())
            {
                case Status.Running:
                    return Status.Running;

                case Status.Success:
                    return Status.Success;

                default:
                    continue;
            }
        }

        return Status.Failure;
    }
}

public class RandomSelector : PrioritySelector
{
    protected override List<Node> SortChildren() => children.Shuffle().ToList();
}

public class Inverter : Node
{ 
    public Inverter(string _name) : base(_name) { }

    public override Status Process()
    {
        switch(children[0].Process())
        {
            case Status.Running: 
                return Status.Running;

            case Status.Failure: 
                return Status.Success;

            default: 
                return Status.Failure;
        }
    }
}

public class UntilFail : Node
{
    public UntilFail(string _name) : base(_name) { }

    public override Status Process()
    {
        if (children[0].Process() == Status.Failure)
        {
            Reset();
            return Status.Failure;
        }

        return Status.Running;
    }
}

public class Repeat : Node
{
    int repititions;
    public Repeat(string _name, int _repititions) : base(_name) 
    { 
        repititions = _repititions;
    }

    public override Status Process()
    {
        int currentRep = 0;
        switch(children[0].Process())
        {
            case Status.Running:
                return Status.Running;

            case Status.Failure:
                return Status.Failure;

            default:
                if(repititions < 0)
                {
                    Reset();
                    return Status.Running;
                }

                currentRep++;
                if(currentRep < repititions)
                {
                    Reset();
                    return Status.Running;
                }
                return Status.Success;
        }
    }
}