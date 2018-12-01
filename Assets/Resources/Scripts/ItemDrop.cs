using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour {

    //Prepares an item for dropping and instantiates the item in the scene
    public void DropItem(GameObject g, float lifetime, float cooldown)
    {       
        try
        {
            //Position of the instantiated pickup
            Vector3 pos = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.2f, this.gameObject.transform.position.z);
            //If the item is not a pickup, create a pickup and assign the item to it
            if (g.GetComponent<Pickup>() == null)
            {
                GameObject pickup = Instantiate(Resources.Load(FileDir.ItemPickup) as GameObject, pos, Quaternion.Euler(45f, 45f, 45f));
                pickup.SetActive(false);
                pickup.GetComponent<Pickup>().SetProperties(lifetime, cooldown);
                pickup.GetComponent<Pickup>().item = Instantiate(g);
                if (g.GetComponent<HealingItem>() != null)
                {
                    pickup.gameObject.GetComponent<MeshRenderer>().material.color = new Color(255f, 0f, 0f);
                }
                pickup.SetActive(true);
                pickup.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f);
            }
            else
            {
                GameObject b = Instantiate(g, pos, Quaternion.Euler(45f, 45f, 45f));;
                b.GetComponent<Rigidbody>().AddForce(Vector3.up * 10);
                b.GetComponent<Pickup>().SetProperties(60f, 0f);
            }
        }

        catch
        {
            Debug.Log("ERROR: couldn't drop item");
            throw;
        }
    }
}
