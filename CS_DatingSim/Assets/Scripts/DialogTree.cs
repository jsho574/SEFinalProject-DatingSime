using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    class Node 
    {
        private string name;
        private string dialog;
        private string[] choiceList;
        private bool choice;
        private int nodeId;
        private int actions;
        private List<Node> child;
        public Node(int nodeId, string name, string dialog, bool choice, int oneChild = 1)
        {
            this.nodeId = nodeId;
            this.name = name;
            this.dialog = dialog;
            this.choice = choice;
            this.actions = 0;
            this.child = new List<Node>(oneChild);
        }
        public Node(int nodeId, string name, string dialog, string[] choiceList, bool choice, int actions)
        {
            this.nodeId = nodeId;
            this.name = name;
            this.dialog = dialog;
            this.choiceList = choiceList;
            this.choice = choice;
            this.actions = actions;
            this.child = new List<Node>(actions);
        }

        public List<Node> Child
        {
            get { return child; }
        }

        public string[] ChoiceList
        {
            get { return choiceList; }
        }
        public bool Choice
        {
            get { return choice; }
        }
        public int NodeId
        {
            get { return nodeId; }
        }
        public int Actions
        {
            get { return actions; }
        }
        public string Name
        {
            get { return name; }
        }
        public string Dialog
        {
            get { return dialog; }
        }
    }

    class DialogTree
    {
        private string[] text;
        private Node root;
        private int lineCount;
        private int finalNodeId;
        public DialogTree(string path)
        {
            text = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, path));
            lineCount = text.Length;
            root = CreateTree(text);
        }

        public Node Root
        {
            get { return root; }
        }
        public int LineCount
        {
            get { return lineCount; }
        }
        public int FinalNodeId
        {
            get { return finalNodeId; }
        }

        private Node CreateTree(string[] text)
        {
            char[] delim = { ':' };
            string[] dialog = text[0].Split(delim);
            if (dialog.Length > 2)
            {
                return null;
            }
            Node root = new Node(0, dialog[0], dialog[1], false);
            Node parent = root;
            Node child = null;
            List<Node> branchEnds = null;
            bool actionsCreated = false;

            for (int i = 1; i < text.Length; i++)
            {
                if (text[i].Equals("<CHOICE>") || text[i].Equals("<choice>"))
                {
                    i++;
                    dialog = text[i].Split(delim);
                    i++;
                    int action;
                    int.TryParse(dialog[1], out action);
                    string[] list = new string[action];
                    for (int y = 0; y < action; y++, i++)
                    {
                        dialog = text[i].Split(delim);
                        list[y] = dialog[1];
                    }
                    if (text[i].Equals("</CHOICE>") || text[i].Equals("</choice>"))
                    {
                        i++;
                    }
                    child = new Node(i, "", "", list, true, action);
                    parent.Child.Add(child);
                    parent = child;
                    branchEnds = new List<Node>(action);
                    for (int z = 0; z < action; z++)
                    {
                        if (text[i].Equals("</ACTION>") || text[i].Equals("</action>"))
                        {
                            i++;
                        }
                        if (text[i].Equals("<ACTION>") || text[i].Equals("<action>"))
                        {
                            i++;
                            dialog = text[i].Split(delim);
                            Node actionChild = new Node(i, dialog[0], dialog[1], false);
                            Node branch = actionChild;
                            Node actionParent = actionChild;
                            while (true)
                            {
                                if (text[i].Equals("</ACTION>") || text[i].Equals("</action>"))
                                {
                                    break;
                                }
                                dialog = text[i].Split(delim);
                                actionChild = new Node(i, dialog[0], dialog[1], false);
                                actionParent.Child.Add(actionChild);
                                actionParent = actionChild;
                                i++;
                            }
                            parent.Child.Add(branch);
                            branchEnds.Add(actionChild);
                        }
                    }
                    actionsCreated = true;
                }
                else
                {
                    
                    dialog = text[i].Split(delim);
                    child = new Node(i, dialog[0], dialog[1], false);
                    if (actionsCreated == true)
                    {
                        if (branchEnds != null)
                        {
                            for (int b = 0; b < branchEnds.Capacity; b++)
                            {
                                branchEnds.ElementAt(b).Child.Add(child);
                            }
                            parent = child;
                        }
                        actionsCreated = false;
                    }
                    else
                    {
                        parent.Child.Add(child);
                        parent = child;
                    }
                }
            }
            if (child != null)
            {
                finalNodeId = child.NodeId;
            } else
            {
                finalNodeId = root.NodeId;
            }
            return root;
        }
    }
}