using System;
using System.Collections;
using System.Collections.Generic;
using Scripts_for_Objects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Game_Logic {
    public class MugController : MonoBehaviour {
        public GameManager gameManager;
        
        public CoffeeRecipe[] Recipes;
        private CoffeeRecipe chosenRecipe;

        public Vector3 Destination;


        private float pouringSpeed;

        private int liquidsAmount;
        private int liquidOrder;

        private GameObject pouredLiquid;
        private GameObject previousLiquid;
        private List<GameObject> pouredLiquids;
        private GameObject[] lines;

        private bool liquidsMoving;
        private bool liquidPouring;
        public bool mugOnPlace;

        private UnityEvent coffeeSold;

        private ClientScaleCounter scaleCounter;
        private ParticleSystem liquidSpamer;
        private Animator arrow;
        private AudioSource pouringSound;
        private AudioSource sellingSound;
        
        private static readonly int LiquidPouring = Animator.StringToHash("liquidPouring");

        void Start() {
            gameManager = GameObject.Find("Dummy").GetComponent<GameManager>();
            liquidSpamer = GameObject.Find("Liquid Spamer").GetComponent<ParticleSystem>();
            arrow = GameObject.Find("Pivot").GetComponent<Animator>();
            pouringSound = GameObject.Find("Pouring Sound").GetComponent<AudioSource>();
            sellingSound = GameObject.Find("Purchase Sound").GetComponent<AudioSource>();
                
            arrow.SetBool(LiquidPouring, false);

            coffeeSold = new UnityEvent();
            pouringSpeed = gameManager.difficulty;

            liquidOrder = 0;

            var randIndex = Random.Range(0, Recipes.Length);
            chosenRecipe = Recipes[randIndex];

            liquidsAmount = chosenRecipe.liquids.Length;
            pouredLiquids = new List<GameObject>();
            lines = new GameObject[liquidsAmount];
            
            if (!mugOnPlace) {
                StartCoroutine(MoveMug(Destination));
            }

            EnableLines();
        }

        private void EnableLines() {
            for (var i = 0; i < liquidsAmount; ++i) {
                var line = ObjectPooler.SharedInstance.GetPooledObject(1);
                var transformPosition = new Vector3(transform.position.x - 0.4224f, 0f, transform.position.z - 0.4296f);
                transformPosition.y = chosenRecipe.proportions[i];

                if (i != liquidsAmount - 1) {
                    var randDelta = Random.Range(-0.3f, 0.3f);
                    transformPosition.y += randDelta;
                }

                if (!mugOnPlace) {
                    transformPosition.x += -0.1977547f;
                }
                
                line.transform.position = transformPosition;

                line.SetActive(true);
                
                if (!mugOnPlace) {
                    StartCoroutine(MoveLine(line.transform, Destination));
                }

                lines[i] = line.gameObject;
            }
        }

        void FixedUpdate() {
            if (!gameManager.sessionAlive) return;
            if (mugOnPlace && liquidOrder < liquidsAmount) {
                //кружка на месте и не все жидкости налиты
                if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && !liquidPouring) {
                    //заходим, когда создаем жидкость
                    arrow.SetBool(LiquidPouring, false);
                    pouringSound.Stop();
                    
                    pouredLiquid = ObjectPooler.SharedInstance.GetPooledObject(0);
                    pouredLiquid.transform.position = new Vector3(0.5303f, -1.1329f, -1.1333f);
                    pouredLiquid.transform.localScale = new Vector3(1, 0.01f, 1);

                    if (liquidOrder != 0) {
                        var v = previousLiquid.transform.position;
                        v.y += previousLiquid.transform.localScale.y;
                        pouredLiquid.transform.position = v;
                    }

                    var liqRenderer = pouredLiquid.GetComponentInChildren<Renderer>();
                    liqRenderer.material = chosenRecipe.liquids[liquidOrder];

                    var liquidSpamerMain = liquidSpamer.main;
                    liquidSpamerMain.startColor = new ParticleSystem.MinMaxGradient(chosenRecipe.liquids[liquidOrder].color);
                        
                    pouredLiquid.SetActive(true);
                    liquidPouring = true;
                }
                else if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) {
                    //заходим, когда жидкость наливается
                    if (pouredLiquid.transform.position.y + pouredLiquid.transform.localScale.y <= 2.2f) {
                        if (!pouringSound.isPlaying) {
                            pouringSound.Play();
                        }

                        pouredLiquid.transform.localScale += new Vector3(0f, pouringSpeed, 0f);
                        liquidSpamer.Emit(10);
                        
                        arrow.SetBool(LiquidPouring, true);
                    }
                    else {
                        //игрок долил жидкость до верхней границы
                        arrow.SetBool(LiquidPouring, false);
                        pouringSound.Stop();
                        
                        scaleCounter = GameObject.Find("Client Scale").GetComponent<ClientScaleCounter>();
                        var liquidY = pouredLiquid.transform.position.y + pouredLiquid.transform.localScale.y;
                        var lineY = lines[liquidOrder].transform.position.y;
                        scaleCounter.CountProportionPoints(liquidY, lineY);
                        
                        previousLiquid = pouredLiquid;
                        pouredLiquids.Add(previousLiquid);

                        foreach (var line in lines) {
                            line.SetActive(false);
                        }

                        liquidOrder = liquidsAmount;
                    }
                }
                else if (liquidPouring) {
                    //заходим, когда надо сменить жидкость
                    arrow.SetBool(LiquidPouring, false);
                    pouringSound.Stop();
                    
                    scaleCounter = GameObject.Find("Client Scale").GetComponent<ClientScaleCounter>();
                    var liquidY = pouredLiquid.transform.position.y + pouredLiquid.transform.localScale.y;
                    var lineY = lines[liquidOrder].transform.position.y;
                    scaleCounter.CountProportionPoints(liquidY, lineY);

                    liquidPouring = false;

                    previousLiquid = pouredLiquid;
                    pouredLiquids.Add(previousLiquid);

                    lines[liquidOrder].SetActive(false);

                    liquidOrder++;
                }
            }
            else {
                if (!liquidsMoving && pouredLiquids.Count > 0) {
                    //заходим, когда кофе уходит клиенту
                    arrow.SetBool(LiquidPouring, false);
                    pouringSound.Stop();
                    sellingSound.Play();

                    foreach (var go in pouredLiquids) {
                        var t = go.transform;
                        var newDest = new Vector3(t.position.x + 9, t.position.y, t.position.z);
                        StartCoroutine(MoveLiquid(t, newDest));
                    }

                    var newDestination = new Vector3(Destination.x + 9, Destination.y, Destination.z);
                    StartCoroutine(MoveMug(newDestination));

                    liquidsMoving = true;

                    var moneyManager = GameObject.Find("Money").GetComponent<MoneyManager>();
                    moneyManager.EarnMoney(pouredLiquids.Count * 2);
                    
                    
                    scaleCounter = GameObject.Find("Client Scale").GetComponent<ClientScaleCounter>();
                    scaleCounter.AddSatisfaction();

                    var score = GameObject.Find("Score").GetComponent<ScoreCounter>();
                    coffeeSold.AddListener(score.OnCoffeeSold);
                    coffeeSold.Invoke();
                }
            }
        }

        private IEnumerator MoveMug(Vector3 dest) {
            for (var i = 0; i < 50; ++i) {
                transform.position += new Vector3(0.2f, 0, 0);
                yield return new WaitForSeconds(0.005f);
            }
            mugOnPlace = !mugOnPlace;

            if (!mugOnPlace) {
                Destroy(gameObject);
            }
        }

        private IEnumerator MoveLiquid(Transform t, Vector3 dest) {
            for (var i = 0; i < 50; ++i) {
                t.position += new Vector3(0.2f, 0, 0);
                yield return new WaitForSeconds(0.005f);
            }

            t.gameObject.SetActive(false);
        }

        private IEnumerator MoveLine(Transform t, Vector3 dest) {
            for (var i = 0; i < 50; ++i) {
                t.position += new Vector3(0.2f, 0, 0);
                yield return new WaitForSeconds(0.005f);
            }
        }

        private void OnDestroy() {
            MugSpawner.MugSpawned = false;
        }
    }
}