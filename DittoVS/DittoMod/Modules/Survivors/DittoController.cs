using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DittoMod.Modules.Survivors
{
	public class DittoController : MonoBehaviour
	{
		public float maxTrackingDistance = 40f;
		public float maxTrackingAngle = 30f;
		public float trackerUpdateFrequency = 10f;
		private HurtBox trackingTarget;
		private CharacterBody characterBody;
		private InputBankTest inputBank;
		private float trackerUpdateStopwatch;
		private Indicator indicator;
		public bool transformed;
		private readonly BullseyeSearch search = new BullseyeSearch();
		private CharacterMaster characterMaster;
		private CharacterBody origCharacterBody;
		private string origName;
		public DittoMasterController dittomastercon;


		private int buffCountToApply;


		private void Awake()
		{
			indicator = new Indicator(gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/HuntressTrackingIndicator"));
			//On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
			
			characterBody = gameObject.GetComponent<CharacterBody>();
			inputBank = gameObject.GetComponent<InputBankTest>();


			buffCountToApply = 0;

		}

		private void Start()
		{


			characterMaster = characterBody.master;
            dittomastercon = characterMaster.gameObject.GetComponent<DittoMasterController>();
            if (!dittomastercon)
			{
				dittomastercon = characterMaster.gameObject.AddComponent<DittoMasterController>();
			}


			//dittomastercon.assaultvest = false;
			//dittomastercon.choiceband = false;
			//dittomastercon.choicescarf = false;
			//dittomastercon.choicespecs = false;
			//dittomastercon.leftovers = false;
			//dittomastercon.lifeorb = false;
			//dittomastercon.luckyegg = false;
			//dittomastercon.rockyhelmet = false;
			//dittomastercon.scopelens = false;
			//dittomastercon.shellbell = false;
			//dittomastercon.assaultvest2 = false;
			//dittomastercon.choiceband2 = false;
			//dittomastercon.choicescarf2 = false;
			//dittomastercon.choicespecs2 = false;
			//dittomastercon.leftovers2 = false;
			//dittomastercon.lifeorb2 = false;
			//dittomastercon.luckyegg2 = false;
			//dittomastercon.rockyhelmet2 = false;
			//dittomastercon.scopelens2 = false;
			//dittomastercon.shellbell2 = false;

		}

		public HurtBox GetTrackingTarget()
		{
			return this.trackingTarget;
		}

		private void OnEnable()
		{
			this.indicator.active = true;
		}

		private void OnDisable()
		{
			this.indicator.active = false;
		}
		//public void AddToBuffCount(int numbertoadd)
		//{
		//	buffCountToApply += numbertoadd;
		//}
		//public int GetBuffCount()
		//      {
		//          return buffCountToApply;
		//      }

		private void FixedUpdate()
		{
			this.trackerUpdateStopwatch += Time.fixedDeltaTime;
			if (this.trackerUpdateStopwatch >= 1f / this.trackerUpdateFrequency)
			{
				this.trackerUpdateStopwatch -= 1f / this.trackerUpdateFrequency;
				HurtBox hurtBox = this.trackingTarget;
				Ray aimRay = new Ray(this.inputBank.aimOrigin, this.inputBank.aimDirection);
				this.SearchForTarget(aimRay);
				this.indicator.targetTransform = (this.trackingTarget ? this.trackingTarget.transform : null);
			}

        }

		private void SearchForTarget(Ray aimRay)
		{
			this.search.teamMaskFilter = TeamMask.all;
			this.search.filterByLoS = true;
			this.search.searchOrigin = aimRay.origin;
			this.search.searchDirection = aimRay.direction;
			this.search.sortMode = BullseyeSearch.SortMode.Distance;
			this.search.maxDistanceFilter = this.maxTrackingDistance;
			this.search.maxAngleFilter = this.maxTrackingAngle;
			this.search.RefreshCandidates();
			this.search.FilterOutGameObject(base.gameObject);
			this.trackingTarget = this.search.GetResults().FirstOrDefault<HurtBox>();
		}




	}
}

