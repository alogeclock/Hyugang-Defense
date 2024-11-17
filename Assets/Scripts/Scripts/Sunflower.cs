
using UnityEngine;

public class Sunflower : Plant
{
    public float produceDuration = 5;
    private float produceTimer = 0;
    private Animator anim;
    public GameObject sunPrefab;

    public float jumpMinDistance = 0.3f;
    public float jumpMaxDistance = 2;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    protected override void EnableUpdate()
    {
        produceTimer += Time.deltaTime;

        if (produceTimer > produceDuration)
        {
            produceTimer = 0;
            anim.SetTrigger("IsGlowing");
        }
    }

    public void ProduceSun()
    { 
        GameObject go= GameObject.Instantiate(sunPrefab, transform.position, Quaternion.identity);

        float distance = Random.Range(jumpMinDistance, jumpMaxDistance);
        distance = Random.Range(0, 2) < 1 ? -distance : distance;
        Vector3 positon = transform.position;
        positon.x += distance;

        go.GetComponent<Sun>().JumpTo(positon);
    }
}
