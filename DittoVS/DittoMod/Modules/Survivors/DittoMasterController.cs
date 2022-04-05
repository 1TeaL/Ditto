//using RoR2;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEngine;

//namespace DittoMod.Modules.Survivors
//{
//	[RequireComponent(typeof(CharacterBody))]
//	[RequireComponent(typeof(TeamComponent))]
//	[RequireComponent(typeof(InputBankTest))]
//	public class DittoMasterController : MonoBehaviour
//	{
//		public float maxTrackingDistance = 40f;
//		public float maxTrackingAngle = 30f;
//		public float trackerUpdateFrequency = 10f;
//		private HurtBox trackingTarget;
//		private CharacterBody characterBody;
//		private InputBankTest inputBank;
//		private float trackerUpdateStopwatch;
//		private Indicator indicator;
//        public bool transformed;
//        private readonly BullseyeSearch search = new BullseyeSearch();
//		private CharacterMaster characterMaster;
//		private CharacterBody origCharacterBody;
//		private string origName;

//		private void Awake()
//		{
//			On.RoR2.HealthComponent.TakeDamage += HealthComponent_TakeDamage;
//		}

//		private void Start()
//		{
//			characterMaster = gameObject.GetComponent<CharacterMaster>();


//			transformed = false;
//		}

//		public HurtBox GetTrackingTarget()
//		{
//			return this.trackingTarget;
//		}

//		private void FixedUpdate()
//		{
//			this.trackerUpdateStopwatch += Time.fixedDeltaTime;


//            if (!characterBody)
//			{
//				characterBody = characterMaster.GetBody();
//			}
//			if (!inputBank)
//            {
//				inputBank = characterBody.gameObject.GetComponent<InputBankTest>();
//			}
//			if (indicator == null)
//			{
//				indicator = new Indicator(characterBody.gameObject, LegacyResourcesAPI.Load<GameObject>("Prefabs/HuntressTrackingIndicator"));
//			}

//			if (this.trackerUpdateStopwatch >= 1f / this.trackerUpdateFrequency)
//			{
//				this.trackerUpdateStopwatch -= 1f / this.trackerUpdateFrequency;
//				HurtBox hurtBox = this.trackingTarget;
//				Ray aimRay = new Ray(this.inputBank.aimOrigin, this.inputBank.aimDirection);
//				this.SearchForTarget(aimRay);
//				this.indicator.targetTransform = (this.trackingTarget ? this.trackingTarget.transform : null);
//			}

//			if(transformed && !characterBody.HasBuff(Modules.Buffs.transformBuff))
//            {
//				RevertCharacter();
//            }
//		}

//		//check if low health
//		private void HealthComponent_TakeDamage(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
//		{
//			orig(self, damageInfo);
//			if (!self) { return; }
//			if (self.health <= 0) { return; }
//			if (self.health <= self.fullCombinedHealth/2 && transformed)
//			{
//				RevertCharacter();
//			}
//		}

//		//Reverts back to first characterbody when called
//		public void RevertCharacter()
//		{
//			transformed = false;
//			characterMaster.bodyPrefab = BodyCatalog.FindBodyPrefab("DittoBody");
//			characterMaster.Respawn(characterMaster.GetBody().transform.position, characterMaster.GetBody().transform.rotation);
//		}

//		private void SearchForTarget(Ray aimRay)
//		{
//			this.search.teamMaskFilter = TeamMask.all;
//			this.search.filterByLoS = true;
//			this.search.searchOrigin = aimRay.origin;
//			this.search.searchDirection = aimRay.direction;
//			this.search.sortMode = BullseyeSearch.SortMode.Distance;
//			this.search.maxDistanceFilter = this.maxTrackingDistance;
//			this.search.maxAngleFilter = this.maxTrackingAngle;
//			this.search.RefreshCandidates();
//			this.search.FilterOutGameObject(base.gameObject);
//            this.trackingTarget = this.search.GetResults().FirstOrDefault<HurtBox>();

//            //List<HurtBox> target = this.search.GetResults().ToList<Hurbox>();
							
			
//		}
	

//	}
//}
