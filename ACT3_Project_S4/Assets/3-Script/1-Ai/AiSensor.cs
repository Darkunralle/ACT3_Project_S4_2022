using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class AiSensor : MonoBehaviour
{
    //Sensor - fait office de champ de vision de l'agent
    [Header("Agent FOV")]
    public float distance = 10;
    public float angle = 30;
    public float height = 1.0f;
    public Color meshColor;
    public int scanFrequency = 30;

    [Header("Layer de scan & cible")]
    //determine le layer du jouer
    public LayerMask Player;
    //determine le layer de l'environnement (mur, arbre, etc)
    public LayerMask occlusionLayers;
    //determine la cible des agent
    public Transform target;
    //determine le bout du fusil de l'agent
    public GameObject cannon;
    //determine la position du joueur A L'INTERIEUR du sensor
    public Vector3 offset;

    [Header("Zone de detection pour definir le statut de l'agent")]
    [Range(0.0f, 1.0f)]
    //le joueur est a porter de tire
    public float engagmentZone;
    //depend de engagmentZone - set a true ou a false
    public bool playerInEngagmentRange;
    [Range(0.0f, 0.10f)]
    //le joueur est au corp a corp
    public float deathZone;
    //depend de deathZone - set a true ou a false
    public bool playerInDeathRange;

    [Header("Liste des élément present dans le champ")]
    //etablie une liste des élément présent dans le sensor
    public List<GameObject> Objects = new List<GameObject>();

    Collider[] colliders = new Collider[50];

    Mesh mesh;
    int count;
    float scanInterval;
    float scanTimer;
    // Start is called before the first frame update
    void Start()
    {
        //interval entre chaque scan
        scanInterval = 1.0f / scanFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        //fait le calcul entre la position du joueur et la position du sensor pour definir l'offset
        offset = transform.position - target.transform.position;

        //cooldown du scan
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }

    private void Scan()
    {
        //sphere de detection de l'engagmentZone
        playerInEngagmentRange = Physics.CheckSphere(transform.position, engagmentZone * 100, Player);
        //sphere de detection de l'deathZone
        playerInDeathRange = Physics.CheckSphere(transform.position, deathZone * 100, Player);
        
        //compte les overlap du sensor
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, Player, QueryTriggerInteraction.Collide);
        
        //clear la liste des objet present dans le sensor
        Objects.Clear();

        //check si des element sont dans le sensor
        for(int i = 0; i < count; ++i)
        {
            GameObject obj = colliders[i].gameObject;
            //si le joueur entre dans le sensor
            if (IsInSight(obj))
            {
                //verifie la distance du player si celui-ci est detecter - change la couleur et le state
                if (playerInEngagmentRange && !playerInDeathRange) 
                { 
                    //Debug.Log("piou piou");
                    meshColor = new Color(1f, 0.5f, 0f, 0.25f);
                }

                else if (playerInDeathRange)
                {
                    //Debug.Log("death piou piou");
                    meshColor = new Color(1f, 0f, 0f, 0.25f);
                }
                else meshColor = new Color(0f, 1f, 0f, 0.25f);

                //adapte le transform du connon pour lui faire regarde le player
                cannon.transform.LookAt(target);
                //ajoute le player a la liste du sensor
                Objects.Add(obj);
                
            }
        }
    }

    //verifie si le joueur entre dans le sensor
    public bool IsInSight(GameObject obj)
    {        
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;

        if (direction.y < 0 || direction.y > height)
        {
            
            return false;
        }

        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);

        if(deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        dest.y = origin.y;

        if (Physics.Linecast(origin, dest, occlusionLayers))
        {
            return false;
        }

        return true;
    }

    //crétion du mesh utiliser pour le sensor
    Mesh CreatWedgeMesh()
    {
        Mesh mesh = new Mesh();

        int segment = 10;
        int numTriangles = (segment * 4) + 2 + 2;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * (height * 3);
        Vector3 topLeft = bottomLeft + Vector3.up * (height * 3);

        int vert = 0;

        //left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segment;
        for(int i = 0; i < segment; ++i)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * (height * 3);
            topLeft = bottomLeft + Vector3.up * (height * 3);
            
            //far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;
        }

        for(int i = 0; i < numVertices; ++i){
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }

    private void OnValidate()
    {
        mesh = CreatWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {

        Debug.DrawLine(cannon.transform.position, cannon.transform.position + cannon.transform.forward * 50, Color.magenta);

        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }

        Gizmos.DrawWireSphere(transform.position, distance);
        for(int i = 0; i < count; ++i)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        }
        Gizmos.color = Color.green;
        foreach(var obj in Objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }
}
