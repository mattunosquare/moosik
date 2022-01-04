import AppSettings from "../Common/AppSettings"
import {IReplyToThreadForm, IReplyToThreadPayload, IPostResourceType} from "../Common/Interfaces"
import {Axios} from "../Common";

const replyToThread = async (formData : IReplyToThreadForm) => {

    const payload : IReplyToThreadPayload = {
        ThreadId: formData.ThreadId,
        UserId: AppSettings.LOGGED_IN_USER_ID,
        Description: formData.Description,
        PostResourceTitle: formData.PostResourceTitle,
        PostResourceValue: formData.PostResourceValue,
        ResourceTypeId: formData.PostResourceTypeId
    }

    try {
        const result = await Axios.post("/post", payload);
        return result.data;
    } catch (error) {
        alert(error);
    }
}

const getAllPostResourceTypes = async () => {
    const result = await Axios.get<IPostResourceType[]>("/post/resourcetypes");
    return result.data;
}

export {
    replyToThread,
    getAllPostResourceTypes
}