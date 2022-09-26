using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class MoveEnemy : MonoBehaviour
{
    [SerializeField] private bool _agro = false;
    [SerializeField] private bool _startAgro = false;
    [SerializeField] private bool _isMove = false;

    [SerializeField] private float _speed;
    [SerializeField] private float koefSpeed = 0;
    [SerializeField] private GameObject _go;
    [SerializeField] private GameObject _target;

    [SerializeField] private GameObject[] targets;

    private bool stopRotate = false;

    // Start is called before the first frame update
    public GameObject FindClosestPlayer()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    void Start()
    {

    }

    void OnCollisionEnter2D(Collision2D collision) //Проверка на столкновения
    {
        if (collision.gameObject.tag == "Wall")
        {
            if (!_agro)
            {
                Debug.Log("Стена!");
                stopRotate = true;
                StartCoroutine(MoveBack());
            } 
        }
    }

    // Update is called once per frame
    void Update()
    {
        _target = FindClosestPlayer();
        TryAgro();

        if (_agro)
        {
            koefSpeed = 1f;
            _isMove = false;
            if (!_startAgro)
            {
                _startAgro = true;
                StartCoroutine(Agro());
            }
            float shortestDistance = Vector3.Distance(transform.position, _target.transform.position);
            if (shortestDistance > 15)
            {
                DEAgro();
            }
        }

        if (!_isMove && !_agro)
        {
            StartCoroutine(Move());
        }
        transform.Translate(0, _speed * koefSpeed * Time.deltaTime, 0);
    }

    IEnumerator Move() //Метод рандомной ходьбы зомби
    {
        if (_agro) yield break;

        _isMove = true;
        float angle = Random.Range(-180, 180);
        int random = Random.Range(0, 2);
        if(random == 0)
        {
            koefSpeed = 0f;
            yield return new WaitForSeconds(Random.Range(0f, 1f));
            _isMove = false;
        }
        else if(random == 1)
        {
            yield return new WaitForSeconds(1f);
            var me = _go.transform;
            var to = me.rotation * Quaternion.Euler(0.0f, 0.0f, angle);

            while (true)
            {
                me.rotation = Quaternion.Lerp(me.rotation, to, 5 * Time.deltaTime);

                if (stopRotate)
                {
                    StopCoroutine(Move());
                }

                if (Quaternion.Angle(me.rotation, to) < 0.01f)
                {
                    me.rotation = to;
                    break;
                }
                yield return null;
            }
            koefSpeed = 0.5f;
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            _isMove = false;
        }
        yield break;
    }
    
    public void TryAgro() //Попытки увидеть игрока в прямой видимости и с определенного расстояния
    {

        float shortestDistance = Vector3.Distance(transform.position, _target.transform.position);
        if(!Physics2D.Linecast(transform.position, _target.transform.position, 7))
        {
            if (shortestDistance < 10)
            {
                StartCoroutine(LookAt(new Vector3(_target.transform.position.x, _target.transform.position.y)));
            }
        }
    }

    IEnumerator MoveBack() //Небольшой метод ходьбы назад
    {
        koefSpeed = -0.3f;
        yield return new WaitForSeconds(0.2f);
        koefSpeed = 0;
        yield break;
    }

    IEnumerator Agro()//метод слежения за игроком
    {
        Vector3 point = new Vector3();
        while (_agro)
        {
            point = new Vector3(_target.transform.position.x, _target.transform.position.y);
            float scalar = point.x * transform.position.x + point.y * transform.position.y;
            float m1 = Mathf.Sqrt(point.x * point.x + point.y * point.y);
            float m2 = Mathf.Sqrt(transform.position.x * transform.position.x + transform.position.y * transform.position.y);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(point.y - transform.position.y, point.x - transform.position.x) * Mathf.Rad2Deg - 90);
            yield return null;
        }
        yield return null;
    }

    IEnumerator LookAt(Vector3 point) //Метод поворота к игроку
    {
        float scalar = point.x * transform.position.x + point.y * transform.position.y;
        float m1 = Mathf.Sqrt(point.x * point.x + point.y * point.y);
        float m2 = Mathf.Sqrt(transform.position.x * transform.position.x + transform.position.y * transform.position.y);
        //transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(point.y - transform.position.y, point.x - transform.position.x) * Mathf.Rad2Deg - 90);
        var me = _go.transform;
        var to = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, Mathf.Atan2(point.y - transform.position.y, point.x - transform.position.x) * Mathf.Rad2Deg - 90);

        while (true)
        {
            me.rotation = Quaternion.Lerp(me.rotation, to, 3 * Time.deltaTime);
            if (Quaternion.Angle(me.rotation, to) < 0.01f)
            {
                me.rotation = to;
                _agro = true;
                yield break;
            }
            yield return null;
        }
    }

    private void DEAgro()
    {
        koefSpeed = 0;
        StopAllCoroutines();
        _agro = false;
        _startAgro = false;
        _isMove = false;
    }
}
