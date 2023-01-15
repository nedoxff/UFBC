using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot.Collections;
using UNDERFELLBattleCreator.addons.underfell_battle_creator;
using UNDERFELLBattleCreator.addons.underfell_battle_creator.Extensions;
using UNDERFELLBattleCreator.addons.underfell_battle_creator.Generator;
using Array = Godot.Collections.Array;

[Tool]
public partial class Editor : Control
{
    private Button _nodesButton => GetNode<Button>("NodesButton");
    private GraphEdit _edit => GetNode<GraphEdit>("GraphEdit");
    private Button _previewButton => GetNode<Button>("PreviewButton");
    
    public static List<GraphNode> SelectedNodes = new(); 
    private TabContainer _nodesContaner => GetNode<TabContainer>("NodesContainer");

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _nodesButton.Connect("pressed", Callable.From(() =>
        {
            if (!_nodesContaner.Visible)
            {
                _nodesContaner.Visible = true;
                _nodesButton.Text = "< Hide nodes";
                _nodesButton.Position -= new Vector2(310, 0);
            }
            else
            {
                _nodesContaner.Visible = false;
                _nodesButton.Text = "Show nodes >";
                _nodesButton.Position += new Vector2(310, 0);
            }
        }));

        _previewButton.Pressed += () =>
        {
            GD.Print(CodeGenerator.Generate(_edit.GetConnectionList()));
        };

        foreach (var child in _nodesContaner.GetChildren())
        {
            var category = child.Name;
            var container = child.GetNode<FlowContainer>("Margin/Container");
            foreach (var node in UnderfellBattleCreator.Instance.Nodes.Where(x => x.Value == category))
            {
                var button = new Button();
                var name = node.Key.Replace(".tscn", "")
                    .Replace($"res://addons/underfell_battle_creator/Scenes/Nodes/{category}/", "")
                    .SplitByCapitalLetter();
                button.Name = name;
                button.Text = name;
                button.Pressed += () =>
                {
                    var graphNode = GD.Load<PackedScene>(node.Key);
                    _edit.AddChild(graphNode.Instantiate());
                };
                container.AddChild(button);
            }
        }

        _edit.ConnectionRequest += OnConnectionRequest;
        _edit.DisconnectionRequest += OnDisconnectRequest;
        _edit.DeleteNodesRequest += OnDeleteRequest;
        _edit.NodeSelected += node => SelectedNodes.Add(node as GraphNode);
        _edit.NodeDeselected += node => SelectedNodes.Remove(node as GraphNode);
    }

    private void OnDeleteRequest(Array nodes)
    {
        foreach(var node in SelectedNodes)
            node.QueueFree();
    }

    private void OnDisconnectRequest(StringName fromNodeName, long fromPort, StringName toNodeName, long toPort)
    {
        var fromNode = _edit.GetNode<GraphNode>(fromNodeName.ToString());
        var toNode = _edit.GetNode<GraphNode>(toNodeName.ToString());
        _edit.DisconnectNode(fromNodeName, (int)fromPort, toNodeName, (int)toPort);
    }

    private void OnConnectionRequest(StringName fromNodeName, long fromPort, StringName toNodeName, long toPort)
    {
        var fromData = NodeData.Data[fromNodeName];
        if (_edit.GetConnectionList().Any(connection =>
                connection["to"].AsString() == toNodeName && connection["to_port"].AsInt64() == toPort &&
                !fromData.AllowMultipleConnections))
        {
            return;
        }

        _edit.ConnectNode(fromNodeName, (int)fromPort, toNodeName, (int)toPort);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _Input(InputEvent @event)
    {
        var justPressed = @event.IsPressed() && !@event.IsEcho();
        if (Input.IsKeyPressed(Key.Shift) && justPressed)
            GD.Print("Searchbar");
    }
}