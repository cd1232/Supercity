using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TileManager : MonoBehaviour {

    public TileController[] m_AllTiles;

    
    private List<TileController> m_NorthTiles;
    private List<TileController> m_CentralTiles;
    private List<TileController> m_WestTiles;
    private List<TileController> m_EastTiles;
    private List<TileController> m_SouthTiles;
    public List<MeshRenderer> m_EmissiveTiles;

    public GameObject EffectsContainer;
    public ParticleSystem PlusParticle;
    public ParticleSystem SmokeParticle;

    // public NavMeshSurface[] surfaces;

    public Material GrassMaterial;

	// Use this for initialization
	void Start () {
        m_AllTiles = FindObjectsOfType<TileController>();
        m_NorthTiles = new List<TileController>();
        m_CentralTiles = new List<TileController>();
        m_WestTiles = new List<TileController>();
        m_EastTiles = new List<TileController>();
        m_SouthTiles = new List<TileController>();

        for (int i = 0; i < m_AllTiles.Length; i++)
        {
            m_AllTiles[i].EffectsObject = EffectsContainer;
            m_AllTiles[i].PlusParticleObject = PlusParticle;
            m_AllTiles[i].SmokeParticle = SmokeParticle;

            ZONE zone = m_AllTiles[i].m_Zone;

            //MeshRenderer[] renderers = transform.parent.GetComponentsInChildren<MeshRenderer>();

            if (zone == ZONE.NORTH)
                m_NorthTiles.Add(m_AllTiles[i]);
            else if (zone == ZONE.CENTER)
                m_CentralTiles.Add(m_AllTiles[i]);
            else if (zone == ZONE.EAST)
                m_EastTiles.Add(m_AllTiles[i]);
            else if (zone == ZONE.WEST)
                m_WestTiles.Add(m_AllTiles[i]);
            else
                m_SouthTiles.Add(m_AllTiles[i]);
        }

      

    }
	
	// Update is called once per frame
	void Update () {

     

    }

    public List<TileController> GetTiles(ZONE _type)
    {
        switch (_type)
        {
            case ZONE.CENTER:
                return m_CentralTiles;
            case ZONE.WEST:
                return m_WestTiles;
            case ZONE.EAST:
                return m_EastTiles;
            case ZONE.NORTH:
                return m_NorthTiles;
            case ZONE.SOUTH:
                return m_SouthTiles;
            default:
                return m_CentralTiles;
        }
    }

    public void SendRubbishTruck(ZONE _type)
    {
        List<TileController> tiles;

        switch (_type)
        {
            case ZONE.CENTER:
                tiles = m_CentralTiles;
                break;
            case ZONE.WEST:
                tiles = m_WestTiles;
                break;
            case ZONE.EAST:
                tiles = m_EastTiles;
                break;
            case ZONE.NORTH:
                tiles = m_NorthTiles;
                break;
            case ZONE.SOUTH:
                tiles = m_SouthTiles;
                break;
            default:
                tiles = m_CentralTiles;
                break;
        }

        foreach (TileController tile in tiles)
        {
            tile.m_bRubbishTruckSent = true;
        }

    }

    public void ResetRubbish()
    {
        foreach (TileController tile in m_AllTiles)
        {
            tile.m_bRubbishTruckSent = false;
        }
    }

    public void StartPublicTransport(ZONE _type)
    {
        List<TileController> tiles;

        switch (_type)
        {
            case ZONE.CENTER:
                tiles = m_CentralTiles;
                break;
            case ZONE.WEST:
                tiles = m_WestTiles;
                break;
            case ZONE.EAST:
                tiles = m_EastTiles;
                break;
            case ZONE.NORTH:
                tiles = m_NorthTiles;
                break;
            case ZONE.SOUTH:
                tiles = m_SouthTiles;
                break;
            default:
                tiles = m_CentralTiles;
                break;
        }

        foreach (TileController tile in tiles)
        {
            tile.m_PublicTransportLevel++;
        }
    }

    public int CheckTransportLevel(ZONE _type)
    {
        switch (_type)
        {
            case ZONE.CENTER:
                if (m_CentralTiles.Count < 1)
                    return 0;
                return m_CentralTiles[0].m_PublicTransportLevel;
            case ZONE.WEST:
                if (m_WestTiles.Count < 1)
                    return 0;
                return m_WestTiles[0].m_PublicTransportLevel; 
            case ZONE.EAST:
                if (m_EastTiles.Count < 1)
                    return 0;
                return m_EastTiles[0].m_PublicTransportLevel; 
            case ZONE.NORTH:
                if (m_NorthTiles.Count < 1)
                    return 0;
                return m_NorthTiles[0].m_PublicTransportLevel;
            case ZONE.SOUTH:
                if (m_SouthTiles.Count < 1)
                    return 0;
                return m_SouthTiles[0].m_PublicTransportLevel;
            default:
                if (m_CentralTiles.Count < 1)
                    return 0;
                return m_CentralTiles[0].m_PublicTransportLevel;
        }
    }
}
