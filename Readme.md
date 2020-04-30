<!-- default file list -->
*Files to look at*:

* [PersistentClasses.cs](./CS/E1883/PersistentClasses.cs) (VB: [PersistentClasses.vb](./VB/E1883/PersistentClasses.vb))
* [Program.cs](./CS/E1883/Program.cs) (VB: [Program.vb](./VB/E1883/Program.vb))
<!-- default file list end -->
# How to create and use the JoinOperand using LINQ to XPO and Criteria Operators


There are three approaches of creating the JoinOperand:

## By using the constructor. 

The JoinOperand has three constructors.The first constructor is parameterless. It can be used if necessary properties are assigned later. These properties are described below in the context of remainder constructors.

The second constructor takes two parameters. `joinTypeName` - the name of a persistent class that is used to join data. The class name can be specified with the namespace if there are classes with equal names. `condition` - the CriteriaOperator that is used to compare keys of the collections that are joined. In addition, there can be used any condition that isn't related to keys.

The third constructor has two additional parameters, allowing you to apply the aggregate expression to the result of the join operation. `type` - the type of the aggregateExpression. This parameter is of the [Aggregate](http://documentation.devexpress.com/#CoreLibraries/DevExpressDataFilteringAggregateEnumtopic) type. `aggregatedExpression` - an expression that is used for calculating values.

## By using the CriteriaOperator.Parse method. 

The general syntax for creating the JoinOperand is: `[<jointypename>][[^.ParentObjectProperty] = [JoinedObjectProperty]]`

## By using the LINQ to XPO. 

The general syntax is the same as the syntax of the Join operation in LINQ:

```cs
from e in employees
join em in employees
on e.Oid equals em.ManagerID
into emg
where emg.Count() > 10
select e;
```
```vb
From e In employees _ 
Group Join em In employees _ 
On e.Oid Equals em.ManagerID _ 
Into emg = Group _ 
Where emg.Count() > 10 _ 
Select e
```