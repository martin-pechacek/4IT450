using Semestralka.DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Semestralka.Other
{
    public class IngredientsOperations
    {
        /**
         *  Return list of ingredients names. Used when recipe detail page is loading
        **/
        public async Task<List<string>> GetIngredientsNames(List<RecipeIngredientsDO> ingredients)
        {
            List<string> ingredientsNamesList = new List<string>();

            //go through ingredients list and get name of each ingredient
            for (int i = 0; i < ingredients.Count; i++)
            {
                int ingredientID = ingredients.ElementAt(i).IngredientID;
                string ingredientName = await IngredientsDO.GetNameAsync(ingredientID);
                ingredientsNamesList.Add(ingredientName);
            }
            return ingredientsNamesList;
        }


        /**
        *  Return list of ingredientsDO. Used when recipe detail page is loading
        **/
        public List<SelectListItem> GetIngredientsDropDown(List<IngredientsDO> ingredientsList)
        {
            List<SelectListItem> ingredientsDropDown = ingredientsList
                .Select(x => new SelectListItem()
                {
                    Text = x.IngredientName,
                    Value = x.IngredientID.ToString()
                })
                .ToList();

            return ingredientsDropDown;
        }

        /**
        *  Return list of units names depending on id.
        **/
        public async Task<List<string>> GetUnitNames(List<RecipeIngredientsDO> ingredients)
        {
            List<string> ingredientsUnitsNamesList = new List<string>();

            //go through ingredients list and get name of each ingredient
            for (int i = 0; i < ingredients.Count; i++)
            {
                int ingredientID = ingredients.ElementAt(i).IngredientID;
                IngredientsDO unitName = await IngredientsDO.GetUnitAsync(ingredientID);
                ingredientsUnitsNamesList.Add(unitName.Unit);
            }

            return ingredientsUnitsNamesList;
        }


    }
}