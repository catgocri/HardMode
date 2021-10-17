/*private void RandomizeMaps()
        {
            foreach (var state in MapLoader.loadedMaps)
            {
                state.potionEffectsOnMap.ForEach(x => x.Status = PotionEffectStatus.Unknown);
            }

            var candidateObjects = new List<RecipeMapItem>();

            candidateObjects.AddRange(FindObjectsOfType<PotionEffectMapItem>());
            candidateObjects.AddRange(FindObjectsOfType<VortexMapItem>());
            candidateObjects.AddRange(FindObjectsOfType<ExperienceBonusMapItem>());

            // FIXME: Support other maps.
            candidateObjects.RemoveAll(mapItem => mapItem.transform.parent.gameObject.name != "RecipeMap Water(Clone)");

            // Collect locations
            var locations = new List<Vector2>();
            foreach (var mapItem in candidateObjects)
            {
                locations.Add(mapItem.transform.localPosition);
            }

            // Randomize locations
            UnityEngine.Random.InitState(DateTime.Now.Millisecond);
            foreach (var mapItem in candidateObjects)
            {
                var index = UnityEngine.Random.Range(0, locations.Count);
                var location = locations[index];
                locations.RemoveAt(index);
                // mapItem.SetPositionOnMap(location);
                mapItem.transform.localPosition = location;
            }

            // Regenerate lines.

            var listOfLines = Reflection.GetPrivateStaticField<DashedLineMapItem, List<DashedLineMapItem>>("listOfLines");
            foreach (var line in listOfLines)
            {
                Destroy(line.gameObject);
            }
            listOfLines.Clear();

            foreach (var map in MapLoader.loadedMaps)
            {
                DashedLineMapItem.CreateLinesOnMap(map);
            }
        }
*/