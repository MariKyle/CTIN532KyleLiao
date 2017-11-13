using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

	//menusub
	public GameObject MenuSub;

	//font
	public GameObject title;
	public GameObject title1;
	public GameObject title2;

	//newspaper headlines
	public GameObject New1;
	public GameObject New2;
	public GameObject New3;
	public GameObject New4;


	//audio
	public GameObject WineTink;
	public GameObject Laugh;
	public GameObject Jazz;
	public GameObject Restaurant;
	public GameObject Open;
	public GameObject Open2;
	public GameObject Thunder;

	//lights
	public GameObject light1;	
	public GameObject light2;
	public GameObject light3;	
	public GameObject light4;
	public GameObject light5;


	//objects
	public GameObject Wine;
	public GameObject Silverware;
	public GameObject ServingDishes;
	public GameObject Table;
	public GameObject chairs;

	public GameObject turkey;
	public GameObject turkey2;
	public GameObject Bread;

	public GameObject Grape1;
	public GameObject Grape2;
	public GameObject Grape3;
	public GameObject Grape4;
	public GameObject Grape5;

	public GameObject Plate1;
	public GameObject Placemat1;
	public GameObject Plate2;
	public GameObject Placemat2;
	public GameObject Plate3;
	public GameObject Placemat3;
	public GameObject Plate4;
	public GameObject Placemat4;
	public GameObject Plate5;
	public GameObject Placemat5;
	public GameObject Plate6;
	public GameObject Placemat6;


		private void Start()
		{
			StartCoroutine(ActivationRoutine());
		}

		private IEnumerator ActivationRoutine()
		{      
		//yield return new WaitForSeconds (1);
		//MenuSub.SetActive (true);
		//yield return new WaitForSeconds (10);
		//MenuSub.SetActive (false);

		yield return new WaitForSeconds(2);
		WineTink.SetActive(true);
		//Wine.SetActive (true);
		title.SetActive (true);

		yield return new WaitForSeconds(1);
		Laugh.SetActive(true);
		New1.SetActive (true);

		yield return new WaitForSeconds(2);
		Restaurant.SetActive (true);
		turkey.SetActive(true);
		Grape1.SetActive (true);
		New2.SetActive (true);

		yield return new WaitForSeconds(1);
		Jazz.SetActive(true);
		Plate1.SetActive (true);
		Placemat1.SetActive (true);
		Grape2.SetActive (true);


		yield return new WaitForSeconds(1);
		Plate2.SetActive (true);
		Placemat2.SetActive (true);
		Grape3.SetActive (true);
		turkey2.SetActive (true);
		Open.SetActive (true);
		New3.SetActive (true);


		yield return new WaitForSeconds(1);
		Plate3.SetActive (true);
		Placemat3.SetActive (true);
		Grape4.SetActive (true);
		Silverware.SetActive (true);
		title.SetActive (false);


		yield return new WaitForSeconds(1);
		Plate4.SetActive (true);
		Placemat4.SetActive (true);
		Grape5.SetActive (true);
		New4.SetActive (true);

		yield return new WaitForSeconds(1);
		Plate5.SetActive (true);
		Placemat5.SetActive (true);
		ServingDishes.SetActive (true);

		yield return new WaitForSeconds(1);
		Plate6.SetActive (true);
		Placemat6.SetActive (true);
		Bread.SetActive (true);

		yield return new WaitForSeconds(1);
		Table.SetActive (true);
		chairs.SetActive (true);
		title1.SetActive (true);

		yield return new WaitForSeconds(10);
		title1.SetActive (false);

		yield return new WaitForSeconds(8);
		Thunder.SetActive (true);
		title2.SetActive (true);
		light1.SetActive (true);
		light2.SetActive (true);
		light3.SetActive (true);
		light4.SetActive (true);
		light5.SetActive (true);

		yield return new WaitForSeconds(2);
		Open2.SetActive (true);

		yield return new WaitForSeconds(14);
		Application.LoadLevel ("FeastOfFools_TableScene");


		}
	}

