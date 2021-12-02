import axios from "axios";

async function axiosTest(){
    const instance = axios.create({ baseURL: "http://localhost:5000/" });

    const response = await instance.get("api/Thread/3",
        {
            headers: {
                'Access-Control-Allow-Origin': '*'
            }
        }
    );

    return response.data;
}



function Dashboard(){

    const testData = axiosTest();

    console.log(testData);
    return(
        <h1>This is the dashboard - </h1>
    );
}
export default Dashboard