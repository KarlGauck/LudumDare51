using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
using System.Linq;

public class Pathfinding : MonoBehaviour
{

    public int posXRange, negXRange, posYRange, negYRange;    
    public Tilemap tilemap;

    private List<Node> graph = new List<Node>();

    // Start is called before the first frame update
    void Start()
    {
        CreateGraph();
    }

    private void CreateGraph()
    {
        Vector3Int gridPosition = tilemap.WorldToCell(new Vector3(transform.position.x, transform.position.y, 0));

        // Add all nodes to graph 
        for (int x = gridPosition.x - negXRange; x <= gridPosition.x + posXRange; x++)
            for (int y = gridPosition.y - negYRange; y <= gridPosition.y + posYRange; y++)
            {
                Vector3Int gridVector = new Vector3Int(x, y, 0);
                if (!tilemap.HasTile(gridVector + new Vector3Int(0, -1, 0)))
                    continue;
                if (tilemap.HasTile(gridVector))
                    continue;

                Node node = new Node();
                node.position = tilemap.CellToWorld(gridVector) + new Vector3(tilemap.cellSize.x*0.5f*tilemap.layoutGrid.transform.localScale.x, tilemap.cellSize.y*0.5f*tilemap.layoutGrid.transform.localScale.y, 0.0f);
                graph.Add(node);
            }
        
        // Connect nodes
        foreach (Node originNode in graph) 
            foreach (Node targetNode in graph)
            {
                if (originNode.Equals(targetNode))
                    continue;

                var distance = Vector3.Distance(targetNode.position, originNode.position);
                if (distance > 3.0)
                    continue;

                var direction = (targetNode.position - originNode.position).normalized;

                var hit = Physics2D.Raycast(new Vector2(originNode.position.x, originNode.position.y), new Vector2(direction.x, direction.y), distance);
                if (hit.distance > distance-0.5)
                    originNode.connections.Add(targetNode);
                else
                {
                    JumpData jumpResult = CalculateBallisticTrajectory(originNode.position, targetNode.position, 0.4f);
                    if (jumpResult == null)
                        continue;

                    originNode.jumpConnections.Add(targetNode, jumpResult);
                }
            }

        // Ensure, all connections are two way
        foreach (Node node in graph)
        {
            foreach (Node neighbour in node.connections)
            {
                if (!neighbour.connections.Contains(node))
                    neighbour.connections.Add(node);
            }
        }

        foreach (Node node in graph)
            foreach (KeyValuePair<Node, JumpData> conn in node.jumpConnections)
            {
                drawJumpPath(conn.Value.velocity, conn.Value.angle, 0.4f, node.position, conn.Key.position);
                Debug.DrawLine(node.position, conn.Key.position, Color.red, 200.05f, false);
            }
        foreach (Node node in graph)
            foreach (Node conn in node.connections)
            {
                Debug.DrawLine(node.position, conn.position, Color.white, 200.05f, false);
            }
    }

    public JumpData CalculateBallisticTrajectory(Vector3 p1, Vector3 p2, float tolerancedist)
    {
        var times = new List<int>();
        var velocities = new List<float>();
        var angles = new List<float>();

        float gravity = 10.0f;
        float dt = 0.01f;

        int timesteps = 80;
        int angleiterations = 10;
        int velocityiterations = 3;
        float maxVelocity = 5.0f;
        float minVelocity = 0.2f;

        float angle = 0.0f;
        float velocity = minVelocity;
        for (int iAngleIt = 0; iAngleIt < angleiterations; iAngleIt ++)
        {
            angle = (float)(0.0 + Math.PI/(angleiterations+1)*(iAngleIt+1));
            if (p2.x-p1.x > 0 && angle > Math.PI/2)
                continue;
            if (p2.x-p1.x < 0 && angle < Math.PI/2)
                continue;

            float dx = (float)Math.Cos(angle);
            float dy = (float)Math.Sin(angle);
            for (int iVelIt = 0; iVelIt < velocityiterations; iVelIt ++)
            {
                velocity = minVelocity + (((float)maxVelocity-minVelocity)/(velocityiterations-1))*iVelIt;
                float xPos = p1.x;
                float yPos = p1.y;
                float yVel =  velocity*dy;
                float xVel = velocity*dx;
                for (int t = 0; t < timesteps; t++)
                {
                    var oldXPos = xPos;
                    var oldYPos = yPos;
                    xPos += xVel * dt;
                    yPos += yVel * dt;
                    //Debug.DrawLine(new Vector3(xPos, yPos, 0), new Vector3(oldXPos, oldYPos, 0), Color.red, 0.05f, false);
                    yVel -= gravity * dt;
                    var vec = new Vector3(xPos, yPos, 0);
                    if (tilemap.HasTile(tilemap.WorldToCell(vec)))
                        break;
                    if (Vector3.Distance(vec, p1) > 5.0)
                        break;
                    if (Vector3.Distance(vec, p2) < tolerancedist)
                    {
                        times.Add(t);
                        velocities.Add(velocity);
                        angles.Add(angle);
                        break;
                    }
                }
            }
        }

        if (times.Count == 0)
            return null;

        List<float> error = new List<float>();
        for (int i = 0; i < times.Count; i++)
        {
            float timeError = (int)times[i] / times.Max();
            float angleError = (float)(Math.Abs((float)angles[i] - Math.PI/2) / (Math.PI/2));
            float velocityError = (float)velocities[i] / velocities.Max();
            error.Add(timeError + angleError + velocityError);
        }
        var index = error.IndexOf(error.Min());
        JumpData data = new JumpData();
        data.angle = (float)angles[index];
        data.velocity = (float)velocities[index];
        return data;
    }

    bool shortestPath(Node start, Node end)
    {
        // A*
        foreach (Node node in graph)
        {
            // Calculate heruistic cost (eudlicean distance to endnode)
            node.hCost = 0.0f; // Dikstra 

            // Reset best gcost
            node.bestGCost = (float)double.PositiveInfinity; 

            // Reset neighbour
            node.bestNeighbour = null;
        }

        List<Node> priorityQue = new List<Node>();
        List<Node> closedList = new List<Node>();

        priorityQue.Add(start);
        bool foundPath = false;
        while(priorityQue.Count != 0)
        {
            print("ContainsEnd: " + priorityQue.Contains(end));
            print("End==Start: " + end.Equals(start));

            Node current = priorityQue.First(x => x.bestGCost + x.hCost == priorityQue.Min(t => t.bestGCost + t.hCost));
            drawCircle(current.position.x, current.position.y, 0.2f);
            if (current == end)
            {
                print(priorityQue.Contains(end));
                foundPath = true;
                break;
            }

            closedList.Add(current);

            print("connections: " + current.connections.Count);
            List<Node> neighbours = current.connections;
            foreach (KeyValuePair<Node, JumpData> conn in current.jumpConnections)
            {
                neighbours.Add((Node)conn.Key);
            }

            foreach (Node neighbour in neighbours)
            {
                bool inQue = priorityQue.Contains(end);

                float directGCost = Vector3.Distance(current.position, neighbour.position);
                if (!current.connections.Contains(neighbour))
                    directGCost *= 1.5f;

                if (closedList.Contains(neighbour) && neighbour.bestGCost + directGCost < current.bestGCost)
                {
                    current.bestGCost = neighbour.bestGCost + directGCost;
                    current.bestNeighbour = neighbour;
                }
                else if (priorityQue.Contains(neighbour) && neighbour.bestGCost > current.bestGCost + directGCost)
                {
                    neighbour.bestGCost = current.bestGCost + directGCost;
                    neighbour.bestNeighbour = current;
                }
                else if (!priorityQue.Contains(neighbour) && !closedList.Contains(neighbour))
                {
                    priorityQue.Add(neighbour);
                    neighbour.bestGCost = current.bestGCost + directGCost;
                    neighbour.bestNeighbour = current;
                }
            }
            closedList.Add(current);
            priorityQue.Remove(current);
        }

        return foundPath;
    } 

    
    private void drawCircle(float x, float y, float r)
    {
        for (int i = 0; i < 10; i++)
        {
            float angle1 = (float)(i * (2*Math.PI/10));
            float angle2 = (float)(angle1 + (2*Math.PI/10));
            float cX1 = (float)(x + Math.Cos(angle1) * r);
            float cY1 = (float)(y + Math.Sin(angle1) * r);
            float cX2 = (float)(x + Math.Cos(angle2) * r);
            float cY2 = (float)(y + Math.Sin(angle2) * r);

            Debug.DrawLine(new Vector3(cX1, cY1, 0), new Vector3(cX2, cY2, 0), Color.magenta, 0.05f, false);
        }
    }

    private void drawJumpPath(float velocity, float angle, float tolerancedist, Vector3 origin, Vector3 target)
    {
        float gravity = 10.0f;
        float dt = 0.01f;
        int timesteps = 80;

        float dx = (float)Math.Cos(angle);
        float dy = (float)Math.Sin(angle);
        float xPos = origin.x;
        float yPos = origin.y;
        float yVel =  velocity*dy;
        float xVel = velocity*dx;
        for (int t = 0; t < timesteps; t++)
        {
            var oldXPos = xPos;
            var oldYPos = yPos;
            xPos += xVel * dt;
            yPos += yVel * dt;
            Debug.DrawLine(new Vector3(xPos, yPos, 0), new Vector3(oldXPos, oldYPos, 0), Color.red, 200.05f, false);
            yVel -= gravity * dt;
            var vec = new Vector3(xPos, yPos, 0);
            if (Vector3.Distance(vec, target) < tolerancedist)
                break;
        }

    }

    public Vector3? getDirection(Vector3 startPos, Vector3 endPos)
    {
        
        Node currentNearestNode = null;
        float nearestDistance = 1000000.0f;
        foreach (Node node in graph)
        {
            float distance = Vector3.Distance(node.position, startPos);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                currentNearestNode = node;
            }
        }

        Node targetNearestNode = null;
        nearestDistance = 1000000.0f;
        foreach (Node node in graph)
        {
            float distance = Vector3.Distance(node.position, endPos);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                targetNearestNode = node;
            }
        }
        if (currentNearestNode == null || targetNearestNode == null)
            return null;
        bool foundPath = shortestPath(targetNearestNode, currentNearestNode);
        if (foundPath && currentNearestNode.bestNeighbour != null)
        {
            return (currentNearestNode.bestNeighbour.position - startPos).normalized;
        }

        return null;
    }

}

class Node
{
    public Vector3 position;
    public List<Node> connections = new List<Node>();
    public IDictionary<Node, JumpData> jumpConnections = new Dictionary<Node, JumpData>();
    public float bestGCost = (float)double.PositiveInfinity; // Positive Infinity
    public float hCost = 0.0f;
    public Node bestNeighbour;
    
}

public class JumpData
{
    public float angle;
    public float velocity;
}
