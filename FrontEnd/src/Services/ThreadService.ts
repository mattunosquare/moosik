import AppSettings from "../Common/AppSettings"
import {IMakeARecommendationForm, IMakeARecommendationPayload} from "../Common/Interfaces"
import {Axios} from "../Common";


const makeARecommendation = async (body :IMakeARecommendationForm) => {
    
    const payload : IMakeARecommendationPayload = {
        Title : body.Title,
        PostDescription: body.PostDescription,
        ThreadTypeId : 2,
        UserId : AppSettings.LOGGED_IN_USER_ID,
        PostResourceTitle : body.PostResourceTitle,
        PostResourceValue: body.PostResourceTitle.concat("VALUE"),
        ResourceTypeId : 1
    }

    try {
        const result = await Axios.post("/thread", payload);
        return result.data;
    } catch (error) {
        alert(error);
    }
}

export {
    makeARecommendation,
};