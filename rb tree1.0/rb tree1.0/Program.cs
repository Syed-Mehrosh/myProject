using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace rb_tree1._0
{
    enum Color
    {
        red,
        black
    }
    class RedBlackTree
    {
        
        public class Node
        {
            public Color clr;
            public Node LeftChild, RightChild, parent;
            public int data;

            public Node(int data) { this.data = data; }
        }
        private Node root;
        public void Insert(int Val)
        {
            Node NewNode = new Node(Val);
            if (root == null)
            {
                root = NewNode;
                root.clr = Color.black;
                return;
            }
            Node Y = null;
            Node X = root;
            while (X != null)
            {
                Y = X;
                if (NewNode.data < X.data)
                {
                    X = X.LeftChild;
                }
                else
                {
                    X = X.RightChild;
                }
            }
            NewNode.parent = Y;
            if (Y == null)
            {
                root = NewNode;
            }
            else if (NewNode.data < Y.data)
            {
                Y.LeftChild = NewNode;
            }
            else
            {
                Y.RightChild = NewNode;
            }
            NewNode.LeftChild = null;
            NewNode.RightChild = null;
            NewNode.clr = Color.red;//clr the new node red
            InsertFixUp(NewNode);//call method to check for violations and fix
        }
        private void InsertFixUp(Node Val)
        {
            //Checks red-black Tree properties
            while (Val != root && Val.parent.clr == Color.red)
            {
                /*We have a violation*/
                if (Val.parent == Val.parent.parent.LeftChild)
                {
                    Node Y = Val.parent.parent.RightChild;
                    if (Y != null && Y.clr == Color.red)//Case 1: uncle is red
                    {
                        Val.parent.clr = Color.black;
                        Y.clr = Color.black;
                        Val.parent.parent.clr = Color.red;
                        Val = Val.parent.parent;
                    }
                    else //Case 2: uncle is black
                    {
                        if (Val == Val.parent.RightChild)
                        {
                            Val = Val.parent;
                            LeftChildRotate(Val);
                        }
                        //Case 3: reclr & rotate
                        Val.parent.clr = Color.black;
                        Val.parent.parent.clr = Color.red;
                        RightChildRotate(Val.parent.parent);
                    }

                }
                else
                {
                    //mirror image of code above
                    Node X = null;

                    X = Val.parent.parent.LeftChild;
                    if (X != null && X.clr == Color.black)//Case 1
                    {
                        Val.parent.clr = Color.red;
                        X.clr = Color.red;
                        Val.parent.parent.clr = Color.black;
                        Val = Val.parent.parent;
                    }
                    else //Case 2
                    {
                        if (Val == Val.parent.LeftChild)
                        {
                            Val = Val.parent;
                            RightChildRotate(Val);
                        }
                        //Case 3: reclr & rotate
                        Val.parent.clr = Color.black;
                        Val.parent.parent.clr = Color.red;
                        LeftChildRotate(Val.parent.parent);

                    }

                }
                root.clr = Color.black;//re-clr the root black as necessary
            }
        }
        private void LeftChildRotate(Node X)
        {
            Node Y = X.RightChild;
            X.RightChild = Y.LeftChild;//turn Y's LeftChild subtree into X's RightChild subtree
            if (Y.LeftChild != null)
            {
                Y.LeftChild.parent = X;
            }
            if (Y != null)
            {
                Y.parent = X.parent;//link X's parent to Y
            }
            if (X.parent == null)
            {
                root = Y;
            }
            if (X == X.parent.LeftChild)
            {
                X.parent.LeftChild = Y;
            }
            else
            {
                X.parent.RightChild = Y;
            }
            Y.LeftChild = X; //put X on Y's LeftChild
            if (X != null)
            {
                X.parent = Y;
            }

        }
        private void RightChildRotate(Node Y)
        {
            // RightChild rotate is simply mirror code from LeftChild rotate
            Node X = Y.LeftChild;
            Y.LeftChild = X.RightChild;
            if (X.RightChild != null)
            {
                X.RightChild.parent = Y;
            }
            if (X != null)
            {
                X.parent = Y.parent;
            }
            if (Y.parent == null)
            {
                root = X;
            }
            if (Y == Y.parent.RightChild)
            {
                Y.parent.RightChild = X;
            }
            if (Y == Y.parent.LeftChild)
            {
                Y.parent.LeftChild = X;
            }

            X.RightChild = Y;//put Y on X's RightChild
            if (Y != null)
            {
                Y.parent = X;
            }
        }
        public void DisplayTree()
        {
            if (root == null)
            {
                Console.WriteLine("Nothing in the tree!");
                return;
            }
            if (root != null)
            {
                InOrderDisplay(root);
            }
        }
        private void InOrderDisplay(Node current)
        {
            if (current != null)
            {
                InOrderDisplay(current.LeftChild);
                Console.Write("({0} color {1}) ->", current.data, current.clr);
                InOrderDisplay(current.RightChild);
            }
        }
        public Node Find(int key)
        {
            bool isFound = false;
            Node temp = root;
            Node Val = null;
            while (!isFound)
            {
                if (temp == null)
                {
                    break;
                }
                if (key < temp.data)
                {
                    temp = temp.LeftChild;
                }
                if (key > temp.data)
                {
                    temp = temp.RightChild;
                }
                if (key == temp.data)
                {
                    isFound = true;
                    Val = temp;
                }
            }
            if (isFound)
            {
                Console.WriteLine("\n{0} was found", key);
                return temp;
            }
            else
            {
                Console.WriteLine("{0} not found", key);
                return null;
            }
        }
        public void Delete(int key)
        {
            //first find the node in the tree to delete and assign to Val pointer/reference
            Node Val = Find(key);
            Node X = null;
            Node Y = null;

            if (Val == null)
            {
                Console.WriteLine("Nothing to delete!");
                return;
            }
            if (Val.LeftChild == null || Val.RightChild == null)
            {
                Y = Val;
            }
            else
            {
                Y = Successor(Val);
            }
            if (Y.LeftChild != null)
            {
                X = Y.LeftChild;
            }
            else
            {
                X = Y.RightChild;
            }
            if (X != null)
            {
                X.parent = Y;
            }
            if (Y.parent == null)
            {
                root = X;
            }
            else if (Y == Y.parent.LeftChild)
            {
                Y.parent.LeftChild = X;
            }
            else
            {
                Y.parent.LeftChild = X;
            }
            if (Y != Val)
            {
                Val.data = Y.data;
            }
            if (Y.clr == Color.black)
            {
                DeleteFixUp(X);
            }

        }
        // Checks the tree for any violations after deletion and performs a fix
        private void DeleteFixUp(Node X)
        {

            while (X != null && X != root && X.clr == Color.black)
            {
                if (X == X.parent.LeftChild)
                {
                    Node W = X.parent.RightChild;
                    if (W.clr == Color.red)
                    {
                        W.clr = Color.black; //case 1
                        X.parent.clr = Color.red; //case 1
                        LeftChildRotate(X.parent); //case 1
                        W = X.parent.RightChild; //case 1
                    }
                    if (W.LeftChild.clr == Color.black && W.RightChild.clr == Color.black)
                    {
                        W.clr = Color.red; //case 2
                        X = X.parent; //case 2
                    }
                    else if (W.RightChild.clr == Color.black)
                    {
                        W.LeftChild.clr = Color.black; //case 3
                        W.clr = Color.red; //case 3
                        RightChildRotate(W); //case 3
                        W = X.parent.RightChild; //case 3
                    }
                    W.clr = X.parent.clr; //case 4
                    X.parent.clr = Color.black; //case 4
                    W.RightChild.clr = Color.black; //case 4
                    LeftChildRotate(X.parent); //case 4
                    X = root; //case 4
                }
                else //mirror code from above with "RightChild" & "LeftChild" exchanged
                {
                    Node W = X.parent.LeftChild;
                    if (W.clr == Color.red)
                    {
                        W.clr = Color.black;
                        X.parent.clr = Color.red;
                        RightChildRotate(X.parent);
                        W = X.parent.LeftChild;
                    }
                    if (W.RightChild.clr == Color.black && W.LeftChild.clr == Color.black)
                    {
                        W.clr = Color.black;
                        X = X.parent;
                    }
                    else if (W.LeftChild.clr == Color.black)
                    {
                        W.RightChild.clr = Color.black;
                        W.clr = Color.red;
                        LeftChildRotate(W);
                        W = X.parent.LeftChild;
                    }
                    W.clr = X.parent.clr;
                    X.parent.clr = Color.black;
                    W.LeftChild.clr = Color.black;
                    RightChildRotate(X.parent);
                    X = root;
                }
            }
            if (X != null)
                X.clr = Color.black;
        }
        private Node Minimum(Node X)
        {
            while (X.LeftChild.LeftChild != null)
            {
                X = X.LeftChild;
            }
            if (X.LeftChild.RightChild != null)
            {
                X = X.LeftChild.RightChild;
            }
            return X;
        }
        private Node Successor(Node X)
        {
            if (X.LeftChild != null)
            {
                return Minimum(X);
            }
            else
            {
                Node Y = X.parent;
                while (Y != null && X == Y.RightChild)
                {
                    X = Y;
                    Y = Y.parent;
                }
                return Y;
            }
        }
        public void printroot()
        {
            Console.WriteLine("\nroot is : "+ root.data);
        }
 
    

    }
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RedBlackTree rbt = new RedBlackTree();
                int a;
                int[] arr = ReadFile(@"C:\Users\Mehrosh\Desktop\rb tree1.0\rb input.txt");
                for (a = 0; a < arr.Length; a++)
                {
                    rbt.Insert(arr[a]);
                }
                rbt.DisplayTree();
                rbt.Find(-1);
                rbt.DisplayTree();
                rbt.Delete(11);
                rbt.DisplayTree();
                rbt.Delete(33);
                rbt.DisplayTree();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static int[] ReadFile(string Integers)
        {
            string[] lines = File.ReadAllLines(@"C:\Users\Mehrosh\Desktop\rb tree1.0\rb input.txt");
            int[] arr = new int[50];
            List<int> az = new List<int>();    
            foreach (string line in lines)
            {
                az.Add(int.Parse(line));
            }
            arr = az.ToArray();
            return arr;
        }
    }
}
